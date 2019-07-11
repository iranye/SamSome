using Mtg.Classes;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Mtg.DataModel
{
    public class MtgContext:DbContext
    {
        public MtgContext()
            :base("Mtg.DataModel.MtgContext")
        { }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Expansion> Expansions { get; set; }
        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
