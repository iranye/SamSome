install-package EntityFramework
install-package EntityFramework.SqlServerCompact

Create DataModel project and add Context.cs class e.g.:
    public class MtgContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
    }

Set DataModel project as startup
After installing EFPowerTools.vsix, in Sln Explorer right-click NinjaContext.cs class and use EntityFramework option to show modeling

reference assembly: System.ComponentModel.DataAnnotations and use [Required] attribute on NinjaEquipment Ninja propery (to get 1 to many vs 0 to many relationship)
enable-migrations
add-migration ce_Initial
update-database -verbose
add-migration ce_Update00




*** App.Config (Sql Server CE)
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
  </configSections>
  <entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlCeConnectionFactory, EntityFramework">
      <parameters>
        <parameter value="System.Data.SqlServerCe.4.0" />
      </parameters>
    </defaultConnectionFactory>
    <providers>
      <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
      <provider invariantName="System.Data.SqlServerCe.4.0" type="System.Data.Entity.SqlServerCompact.SqlCeProviderServices, EntityFramework.SqlServerCompact" />
    </providers>
  </entityFramework>
  <system.data>
    <DbProviderFactories>
      <remove invariant="System.Data.SqlServerCe.4.0" />
      <add name="Microsoft SQL Server Compact Data Provider 4.0" invariant="System.Data.SqlServerCe.4.0" description=".NET Framework Data Provider for Microsoft SQL Server Compact" type="System.Data.SqlServerCe.SqlCeProviderFactory, System.Data.SqlServerCe, Version=4.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" />
    </DbProviderFactories>
  </system.data>
  <connectionStrings>
    <add name="NinjaDomain.DataModel.NinjaContext"
    providerName="System.Data.SqlServerCe.4.0"
    connectionString="Data Source=|DataDirectory|..\..\..\NinjaDomain.DataModel.NinjaContext.sdf" />
  </connectionStrings>
</configuration>