using System;
using System.CodeDom;
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

        public SamuraiContext(String mdfPath)
        {
            if (String.IsNullOrEmpty(mdfPath))
            {
                throw new ArgumentNullException(nameof(mdfPath));
            }
            _mdfPath = mdfPath;
        }

        private string _mdfPath = String.Empty;
        public String MdfPath
        {
            get
            {
                return _mdfPath;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = Directory.GetCurrentDirectory();
            optionsBuilder
                .UseLoggerFactory(MyConsoleLoggerFactory)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(
                $"Data Source=(LocalDB)\\MSSQLLocalDB;Database=SamuraiAppData;AttachDbFilename={MdfPath};Integrated Security=True; Connect Timeout=30;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new {s.SamuraiId, s.BattleId});
        }
    }
}
