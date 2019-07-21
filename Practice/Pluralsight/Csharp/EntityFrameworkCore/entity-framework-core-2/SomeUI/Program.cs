using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using SomeUI.Properties;
using Microsoft.EntityFrameworkCore.Query;

namespace SomeUI
{
    class Program
    {
        private static readonly string MdfPath = Settings.Default.mdfPath;
        private static SamuraiContext _context = new SamuraiContext(MdfPath);

        static void Main(string[] args)
        {
            var option = String.Empty;
            if (args.Length > 0)
            {
                option = args[0].ToLower();
            }
            if (option.StartsWith("p"))
            {
                PrintAllSamurais();
                return;
            }
            if (option.StartsWith("happy"))
            {
                PrintAllSamurais();
                return;
            }
            //InsertSamurai();
            //InsertMultipleSamurai();
            //InsertMultipleDifferentObjects();

            //PrintAllSamurais();
            //QuerySamuraisByName("Julie");

            //RetrieveAndUpdateSamurai();

            //QueryAndUpdateBattle_Disconnected();

            // InsertNewPkFkGraphMultipleChildren();

            //AddChildToExistingObjectWhileTracked();

            // AddChildToExistingObjectWhileNotTracked();

            EagerLoadingSamuraiWithQuotes();
            // FilteringWithRelatedData()
            //PrintAllSamurais();
            Console.Read();
        }

        private static void InsertSamurai()
        {
            Samurai samurai = GetNewSamurai();
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertMultipleSamurai()
        {
            List<Samurai> samurais = new List<Samurai>
            {
                GetNewSamurai("Snarf"),
                GetNewSamurai("Barf")
            };
            _context.Samurais.AddRange(samurais);
            _context.SaveChanges();
        }

        private static void InsertMultipleDifferentObjects()
        {
            var samurai = GetNewSamurai("Oda Nobunaga");
            var city = GetRandomString(CitiesInJapan);
            var battleName = "Battle of " + city;
            var battle = new Battle
            {
                Name = battleName, // "Battle of Nagashino",
                StartDate = new DateTime(1575, 06, 16),
                EndDate = new DateTime(1575, 06, 28)
            };
            _context.AddRange(samurai, battle);
            _context.SaveChanges();
        }

        private static Samurai GetNewSamurai(string baseName=null)
        {
            var rand = new Random();
            var name = String.IsNullOrEmpty(baseName) ? "Julie" : baseName;
            var samuraiName = $"{name}{rand.Next(0, 955)}";
            return new Samurai { Name = samuraiName };
        }

        private static void QuerySamuraisByName(string name)
        {
            //Samurai julieSamurai = _context.Samurais.FirstOrDefault(s => s.Name.StartsWith(name));
            //Console.WriteLine(julieSamurai);

            // var julieSamurais = _context.Samurais.Where(s => s.Name.StartsWith(name)).ToList();
            var julieSamurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, $"{name}%")).ToList();
            foreach (var samurai in julieSamurais)
            {
                Console.WriteLine(samurai);
            }
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            if (samurai != null)
            {
                samurai.Name += "San";
                _context.SaveChanges();
            }
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.FirstOrDefault();
            if (battle == null)
            {
                return;
            }
            battle.EndDate = new DateTime(1600, 03, 04);
            using (var newContextInstance = new SamuraiContext(MdfPath))
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void InsertNewPkFkGraphMultipleChildren()
        {
            var samurai = GetNewSamurai("Kyuzo");
            samurai.Quotes = new List<Quote>
            {
                new Quote{Text="Watch out for my sharp sword!"},
                new Quote{Text = "I told you to watch out for the sword!  Oh Well!"}
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
            PrintAllSamurais(new List<Samurai> { samurai });
        }

        private static void AddChildToExistingObjectWhileTracked()
        {
            var samurai = _context.Samurais.First();
            var randomString = GetRandomString(Quotes);
            samurai.Quotes.Add(new Quote { Text = randomString });
            _context.SaveChanges();
            Console.WriteLine(samurai);
        }

        private static void AddChildToExistingObjectWhileNotTracked()
        {
            var samurai = GetNewSamurai("Nota Ninja");
            _context.Samurais.Add(samurai);
            _context.SaveChanges();

            var happyQuote = "Don't worry, be happy.";
            var randomString = happyQuote + "  " + GetRandomString(Quotes);
            var quote = new Quote
            {
                Text = randomString,
                SamuraiId = samurai.Id
            };
            using (var newContext = new SamuraiContext(MdfPath))
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
            Console.WriteLine(samurai);
        }

        private static void EagerLoadingSamuraiWithQuotes()
        {
            IIncludableQueryable<Samurai, List<Quote>> samurais = _context.Samurais.Include(s => s.Quotes);
            PrintAllSamurais(samurais);
        }

        // Helper Methods
        private static void PrintAllSamurais(IEnumerable<Samurai> samurais=null)
        {
            if (samurais != null)
            {
                samurais.ToList().ForEach(s => Console.WriteLine(s));
                return;
            }

            using (var context = new SamuraiContext(MdfPath))
            {
                foreach (var samurai in context.Samurais.ToList())
                {
                    Console.WriteLine(samurai.ToString());
                }
            }
        }

        private static string GetRandomString(List<string> lst)
        {
            if (lst == null || lst.Count == 0)
            {
                return "foobar";
            }
            var rand = new Random();
            var index = rand.Next(0, lst.Count - 1);
            return lst[index];
        }

        private static List<string> CitiesInJapan = new List<string>
        {
            "Nagoya",
            "Toyohashi",
            "Okazaki",
            "Ichinomiya",
            "Seto",
            "Handa",
            "Kasugai",
            "Toyokawa",
            "Tsushima",
            "Hekinan",
            "Kariya",
            "Toyota",
            "Anjō",
            "Nishio",
            "Gamagōri",
            "Inuyama",
            "Tokoname",
            "Kōnan",
            "Komaki",
            "Inazawa",
            "Tōkai",
            "Ōbu",
            "Chita",
            "Chiryū",
            "Owariasahi",
            "Takahama",
            "Iwakura",
            "Toyoake",
            "Nisshin",
            "Tahara",
            "Aisai",
            "Kiyosu",
            "Shinshiro",
            "Kitanagoya",
            "Yatomi",
            "Miyoshi",
            "Ama",
            "Nagakute",
            "Akita",
            "Ōdate",
            "Kazuno",
            "Daisen",
            "Katagami",
            "Kitaakita",
            "Oga",
            "Yurihonjō",
            "Yuzawa",
            "Semboku",
            "Yokote",
            "Nikaho"
        };

        private static List<string> Quotes = new List<string>
        {
            "Watch out for my sharp sword!",
            "I told you to watch out for the sword!  Oh Well!",
            "Frankly, my dear, I don't give a damn.",
            "I'm going to make him an offer he can't refuse.",
            "You don't understand! I coulda had class. I coulda been a contender. I could've been somebody, instead of a bum, which is what I am.",
            "Toto, I've got a feeling we're not in Kansas anymore.",
            "Here's looking at you, kid.",
            "Go ahead, make my day.",
            "All right, Mr. DeMille, I'm ready for my close-up.",
            "May the Force be with you.",
            "Fasten your seatbelts. It's going to be a bumpy night.",
            "You talking to me?",
            "What we've got here is failure to communicate.",
            "I love the smell of napalm in the morning.",
            "Love means never having to say you're sorry.",
            "The stuff that dreams are made of.",
            "E.T. phone home.",
            "They call me Mister Tibbs!",
            "Rosebud.",
            "Made it, Ma! Top of the world!",
            "I'm as mad as hell, and I'm not going to take this anymore!",
            "Louis, I think this is the beginning of a beautiful friendship.",
            "A census taker once tried to test me. I ate his liver with some fava beans and a nice Chianti.",
            "Bond. James Bond.",
            "There's no place like home.",
            "I am big! It's the pictures that got small.",
            "Show me the money!",
            "Why don't you come up sometime and see me?",
            "I'm walking here! I'm walking here!",
            "Play it, Sam. Play 'As Time Goes By.'",
            "You can't handle the truth!",
            "I want to be alone.",
            "After all, tomorrow is another day!",
            "Round up the usual suspects.",
            "I'll have what she's having.",
            "You know how to whistle, don't you, Steve? You just put your lips together and blow.",
            "You're gonna need a bigger boat.",
            "Badges? We ain't got no badges! We don't need no badges! I don't have to show you any stinking badges!",
            "I'll be back.",
            "Today, I consider myself the luckiest man on the face of the earth.",
            "If you build it, he will come.",
            "Mama always said life was like a box of chocolates. You never know what you're gonna get.",
            "We rob banks.",
            "Plastics.",
            "We'll always have Paris.",
            "I see dead people.",
            "Stella! Hey, Stella!",
            "Oh, Jerry, don't let's ask for the moon. We have the stars.",
            "Shane. Shane. Come back!",
            "Well, nobody's perfect.",
            "It's alive! It's alive!",
            "Houston, we have a problem.",
            "You've got to ask yourself one question: 'Do I feel lucky?' Well, do ya, punk?",
            "You had me at ‘hello.’",
            "One morning I shot an elephant in my pajamas. How he got in my pajamas, I don't know.",
            "There's no crying in baseball!",
            "La-dee-da, la-dee-da.",
            "A boy's best friend is his mother.",
            "Greed, for lack of a better word, is good.",
            "Keep your friends close, but your enemies closer.",
            "As God is my witness, I'll never be hungry again.",
            "Well, here's another nice mess you've gotten me into!",
            "Say “hello” to my little friend!",
            "What a dump.",
            "Mrs. Robinson, you're trying to seduce me. Aren't you?",
            "Gentlemen, you can't fight in here! This is the War Room!",
            "Elementary, my dear Watson.",
            "Get your stinking paws off me, you damned dirty ape.",
            "Of all the gin joints in all the towns in all the world, she walks into mine.",
            "Here's Johnny!",
            "They're here!",
            "Is it safe?",
            "Wait a minute, wait a minute. You ain't heard nothin' yet!",
            "No wire hangers, ever!",
            "Mother of mercy, is this the end of Rico?",
            "Forget it, Jake, it's Chinatown.",
            "I have always depended on the kindness of strangers.",
            "Hasta la vista, baby.",
            "Soylent Green is people!",
            "Open the pod bay doors, HAL.",
            "Surely you can't be serious.",
            "Yo, Adrian!",
            "Hello, gorgeous.",
            "Toga! Toga!",
            "Listen to them. Children of the night. What music they make.",
            "Oh, no, it wasn't the airplanes. It was Beauty killed the Beast.",
            "My precious.",
            "Attica! Attica!",
            "Sawyer, you're going out a youngster, but you've got to come back a star!",
            "Listen to me, mister. You're my knight in shining armor. Don't you forget it. You're going to get back on that horse, and I'm going to be right behind you, holding on tight, and away we're gonna go, go, go!",
            "Tell 'em to go out there with all they got and win just one for the Gipper.",
            "A martini. Shaken, not stirred.",
            "I bet you're happy that I saved you"
        };
    }
}
