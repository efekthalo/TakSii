﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiiTaxi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SiiTaxiEntities : DbContext
    {
        public SiiTaxiEntities()
            : base("name=SiiTaxiEntities")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SiiTaxiEntities>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Approvers> Approvers { get; set; }
        public virtual DbSet<People> People { get; set; }
        public virtual DbSet<Taxi> Taxi { get; set; }
        public virtual DbSet<TaxiPeople> TaxiPeople { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }
    }
}
