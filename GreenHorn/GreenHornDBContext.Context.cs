﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GreenHorn
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GreenHorneDBEntities : DbContext
    {
        public GreenHorneDBEntities()
            : base("name=GreenHorneDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<application> applications { get; set; }
        public virtual DbSet<candidate> candidates { get; set; }
        public virtual DbSet<company> companies { get; set; }
        public virtual DbSet<cvfile> cvfiles { get; set; }
        public virtual DbSet<industry> industries { get; set; }
        public virtual DbSet<position> positions { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
