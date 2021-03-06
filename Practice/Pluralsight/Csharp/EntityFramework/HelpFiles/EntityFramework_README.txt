﻿* DataModel *
Create DataModel project and add Context.cs class e.g.:
    public class MtgContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Expansion> Expansions { get; set; }
    }

Set DataModel project as startup	
install-package EntityFramework
install-package EntityFramework.SqlServerCompact

After installing EFPowerTools.vsix, in Sln Explorer right-click NinjaContext.cs class and use EntityFramework option to show modeling

Update App.config (see below (addition of connectionString section should be all that's needed))
reference assembly: System.ComponentModel.DataAnnotations and use [Required] attribute on NinjaEquipment Ninja propery (to get 1 to many vs 0 to many relationship)
enable-migrations

in Class that sub-classes DbContext add:
protected override void OnModelCreating( DbModelBuilder dbModelBuilder)
{
dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
}

add-migration Initial
update-database -verbose
add-migration ce_AddDob

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


  <connectionStrings>
    <add name="Mtg.DataModel.MtgContext"
    providerName="System.Data.SqlServerCe.4.0"
    connectionString="Data Source=|DataDirectory|..\..\..\Mtg.DataModel.MtgContext.sdf" />
  </connectionStrings>
  
  
  <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|..\..\..\Ninjas.mdf;Initial Catalog=aspnet-GigHub;Integrated Security=True"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
  
  <connectionStrings>
    <add name="myConnectionString" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Mtg.DataModel.MtgContext.mdf;Initial Catalog=Mtg.DataModel.MtgContext;Integrated Security=True"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
  
  <connectionStrings>
    <add name="MtgDataModel" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|MtgDataModel.mdf;Initial Catalog=MtgDataModel;Integrated Security=True"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
  
  <connectionStrings>
  <add name="myConnectionString"
       connectionString="Data Source=VBOX01;Initial Catalog=netnutsandbolts;User ID=netnutsandbolts;Password=netnutsandbolts;MultipleActiveResultSets=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>

  <connectionStrings>
    <add name="Mtg.DataModel.MtgContext" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=App_Data\Mtg.DataModel.MtgContext.mdf;Initial Catalog=Mtg.DataModel.MtgContext;Integrated Security=True"
      providerName="System.Data.SqlClient" />
  </connectionStrings>