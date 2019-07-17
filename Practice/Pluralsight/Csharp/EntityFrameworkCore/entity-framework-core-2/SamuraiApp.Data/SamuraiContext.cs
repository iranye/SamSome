﻿using System;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public static readonly LoggerFactory MyConsoleLoggerFactory
        = new LoggerFactory(new []
        {
            new ConsoleLoggerProvider((category, level)
                => category == DbLoggerCategory.Database.Command.Name
                   && level==LogLevel.Information, true ) 
        });

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        private string _path = String.Empty;
        public String Path
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_path))
                {
                    _path =
                        @"Foobar";
                }

                return _path;
            }
            set { _path = value; }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = Directory.GetCurrentDirectory();
            //var path = Path.Combine(cs, @"App_Data\SamuraiData.mdf");
            var path =
                @"E:\source_git\SamSome\Practice\Pluralsight\Csharp\EntityFrameworkCore\entity-framework-core-2\SomeUI\App_Data\SamuraiData.mdf";
            optionsBuilder
                .UseLoggerFactory(MyConsoleLoggerFactory)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(
                $"Data Source=(LocalDB)\\MSSQLLocalDB;Database=SamuraiAppData;AttachDbFilename={Path};Integrated Security=True; Connect Timeout=30;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new {s.SamuraiId, s.BattleId});
        }
    }
}
