install-package EntityFramework
install-package EntityFramework.SqlServerCompact

Create DataModel project and add Context.cs class e.g.:
    public class MtgContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
    }

Set DataModel project as startup
enable-migrations
add-migration ce_Initial
update-database -verbose
add-migration ce_Update00