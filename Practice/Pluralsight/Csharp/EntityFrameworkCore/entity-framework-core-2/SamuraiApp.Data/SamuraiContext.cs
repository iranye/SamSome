﻿using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        //public static string MDF_Directory
        //{
        //    get
        //    {
        //        var directoryPath = AppDomain.CurrentDomain.BaseDirectory;
        //        return Path.GetFullPath(Path.Combine(directoryPath, "..//..//..//TestDB"));
        //    }
        //}

        public string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|SamuraiData.mdf;Integrated Security=True; Connect Timeout=30;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
