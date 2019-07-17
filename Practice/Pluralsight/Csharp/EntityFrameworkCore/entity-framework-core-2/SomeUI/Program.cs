﻿using System;
using System.Collections.Generic;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUI
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertSamurai();
            //InsertMultipleSamurai();
            //InsertMultipleDifferentObjects();
        }

        private static void InsertMultipleDifferentObjects()
        {
            var samurai = GetNewSamurai("Oda Nobunaga");
            var battle = new Battle
            {
                Name = "Battle of Nagashino",
                StartDate = new DateTime(1575, 06, 16),
                EndDate = new DateTime(1575, 06, 28)
            };
            using (var context = new SamuraiContext())
            {
                context.AddRange(samurai, battle);
                context.SaveChanges();
            }
        }

        private static void InsertMultipleSamurai()
        {
            List<Samurai> samurais = new List<Samurai>
            {
                GetNewSamurai("Snarf"),
                GetNewSamurai("Barf")
            };
            using (var context = new SamuraiContext())
            {
                context.Samurais.AddRange(samurais);
                context.SaveChanges();
            }
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

        private static Samurai GetNewSamurai(string baseName=null)
        {
            var rand = new Random();
            var name = String.IsNullOrEmpty(baseName) ? "Julie" : baseName;
            var samuraiName = $"{name}{rand.Next(0, 955)}";
            return new Samurai { Name = samuraiName };
        }
    }
}
