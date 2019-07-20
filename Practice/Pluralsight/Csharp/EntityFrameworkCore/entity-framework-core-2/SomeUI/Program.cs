using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using SomeUI.Properties;

namespace SomeUI
{
    class Program
    {
        private static readonly string MdfPath = Settings.Default.mdfPath;
        private static SamuraiContext _context = new SamuraiContext(MdfPath);

        static void Main(string[] args)
        {
            //InsertSamurai();
            //InsertMultipleSamurai();
            //InsertMultipleDifferentObjects();

            //PrintAllSamurais();
            //QuerySamuraisByName("Julie");

            RetrieveAndUpdateSamurai();
            PrintAllSamurais();

            QueryAndUpdateBattle_Disconnected();
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
            var city = GetRandomCity();
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

        // Helper Methods
        private static void PrintAllSamurais()
        {
            using (var context = new SamuraiContext(MdfPath))
            {
                foreach (var samurai in context.Samurais.ToList())
                {
                    Console.WriteLine(samurai.ToString());
                }
            }
        }

        private static string GetRandomCity()
        {
            var rand = new Random();
            var cityIndex = rand.Next(0, CitiesInJapan.Count-1);
            return CitiesInJapan[cityIndex];
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
    }
}
