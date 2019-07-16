using System;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUI
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertSamurai();
        }

        private static void InsertSamurai()
        {
            Samurai samurai = GetNewSamurai();
            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }

        private static Samurai GetNewSamurai()
        {
            var rand = new Random();
            var samuraiName = $"Julie{rand.Next(0, 55)}";
            return new Samurai { Name = samuraiName };
        }
    }
}
