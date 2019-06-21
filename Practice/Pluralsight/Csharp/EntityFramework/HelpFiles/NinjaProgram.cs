using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            int clanId = LazyInsertClanFoobar();
            Console.WriteLine("Adding new Ninja to ClanID=" + clanId);
            if (clanId == -1)
            {
                return;
            }
            InsertNinja(clanId);
            PrintAllNinjas();
        }

        private static void InsertNinja(int clanId)
        {
            string ninjaName = $"SampsonSan_{RandomInt()}";
            InsertNinja(ninjaName, clanId);
        }

        private static void InsertNinja(string ninjaName, int clanId)
        {
            if (clanId <= 0)
            {
                Console.WriteLine("Invalid ClanID: " + clanId.ToString());
                return;
            }
            try
            {
                var ninja = new Ninja
                {
                    Name = ninjaName,
                    ServedInOniwaban = false,
                    ClanId = clanId,
                    DateOfBirth = new DateTime(2008, 1, 28)
                };
                using (var context = new NinjaContext())
                {
                    context.Database.Log = Console.WriteLine;
                    context.Ninjas.Add(ninja);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static int LazyInsertClanFoobar()
        {
            int clanId = -1;
            string clanName = "Foobar";
            try
            {
                using (var context = new NinjaContext())
                {
                    context.Database.Log = Console.WriteLine;
                    var foobarClan = context.Clans.FirstOrDefault(c => c.ClanName == clanName);
                    if (foobarClan == null)
                    {
                        foobarClan = new Clan
                        {
                            ClanName = clanName
                        };
                        context.Clans.Add(foobarClan);
                        clanId = context.SaveChanges();
                    }
                    else
                    {
                        clanId = foobarClan.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return clanId;
        }

        private static void PrintAllNinjas()
        {
            try
            {
                using (var context = new NinjaContext())
                {
                    foreach (var ninja in context.Ninjas)
                    {
                        Console.WriteLine(ninja);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                    .Where(n => n.DateOfBirth >= new DateTime(1984, 1, 1))
                    .OrderBy(n => n.Name)
                    .Skip(1).Take(1);

                //var query = context.Ninjas;
                // var someninjas = query.ToList();
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);
                context.SaveChanges();
            }
        }

        private static int RandomInt()
        {
            var rand = new Random();
            return rand.Next(100);
        }
    }
}
