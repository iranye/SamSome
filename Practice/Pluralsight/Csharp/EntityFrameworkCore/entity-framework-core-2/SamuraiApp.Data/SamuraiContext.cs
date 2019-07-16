using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System.IO;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = Directory.GetCurrentDirectory();
            var path = Path.Combine(cs, @"App_Data\SamuraiData.mdf");
            optionsBuilder.UseSqlServer(
                $"Data Source=(LocalDB)\\MSSQLLocalDB;Database=SamuraiAppData;AttachDbFilename={path};Integrated Security=True; Connect Timeout=30;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
