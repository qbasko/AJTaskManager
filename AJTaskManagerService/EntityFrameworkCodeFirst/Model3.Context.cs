﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityFrameworkCodeFirst
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities2 : DbContext
    {
        public Entities2()
            : base("name=Entities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<ExternalUsers> ExternalUsers { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<RoleTypes> RoleTypes { get; set; }
        public virtual DbSet<TaskItems> TaskItems { get; set; }
        public virtual DbSet<TaskStatus> TaskStatus { get; set; }
        public virtual DbSet<TaskSubitems> TaskSubitems { get; set; }
        public virtual DbSet<TaskSubitemWorks> TaskSubitemWorks { get; set; }
        public virtual DbSet<TodoItems> TodoItems { get; set; }
        public virtual DbSet<TodoLists> TodoLists { get; set; }
        public virtual DbSet<UserDomains> UserDomains { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
        public virtual DbSet<UserNotifications> UserNotifications { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
