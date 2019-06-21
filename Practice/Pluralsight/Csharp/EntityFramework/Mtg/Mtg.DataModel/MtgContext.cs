using Mtg.Classes;
using System.Data.Entity;

namespace Mtg.DataModel
{
    public class MtgContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Expansion> Expansions { get; set; }
    }
}
