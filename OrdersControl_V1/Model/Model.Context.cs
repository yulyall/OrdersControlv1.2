﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrdersControl_V1.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OrderControlEntities : DbContext
    {
        public OrderControlEntities()
            : base("name=OrderControlEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Mark> Mark { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Status> Status { get; set; }
    }
}
