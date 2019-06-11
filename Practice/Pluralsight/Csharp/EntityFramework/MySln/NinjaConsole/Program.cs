using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;

namespace NinjaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertNinja();
            Console.ReadKey();
        }

        private static void InsertNinja()
        {
            // No Clan w/ ClanID=1 exists
            var ninja = new Ninja
            {
                Name = "SampsonSan",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(2008, 1, 28),
                ClanId = 1

            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }

    }
}
