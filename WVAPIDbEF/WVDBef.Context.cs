﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WVAPIDbEF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class wvDB : DbContext
    {
        public wvDB()
            : base("name=wvDB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<IBLongShortRatio> IBLongShortRatios { get; set; }
        public virtual DbSet<IBStrategy> IBStrategies { get; set; }
        public virtual DbSet<IBStrategyMapping> IBStrategyMappings { get; set; }
        public virtual DbSet<IBCashActivity> IBCashActivities { get; set; }
    }
}