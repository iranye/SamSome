﻿using Mtg.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtg.DataModel
{
    public class MtgContext : DbContext
    {
        public MtgContext()
            : base("Mtg.DataModel.MtgContext")
        { }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Expansion> Expansions { get; set; }
        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
