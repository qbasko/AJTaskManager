﻿using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using AJTaskManagerServiceService.DataObjects;

namespace AJTaskManagerServiceService.Models
{
    public class AJTaskManagerServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.
        private const string connectionStringName = "Name=MS_TableConnectionString";

        public AJTaskManagerServiceContext()
            : base(connectionStringName)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<TaskStatus> TaskStatus { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskSubitem> TaskSubitems { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schema = ServiceSettingsDictionary.GetSchemaName();
            if (!string.IsNullOrEmpty(schema))
            {
                modelBuilder.HasDefaultSchema(schema);
            }
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }

        public System.Data.Entity.DbSet<AJTaskManagerServiceService.DataObjects.TodoList> TodoLists { get; set; }

        public System.Data.Entity.DbSet<AJTaskManagerServiceService.DataObjects.UserDomain> UserDomains { get; set; }

        public System.Data.Entity.DbSet<AJTaskManagerServiceService.DataObjects.ExternalUser> ExternalUsers { get; set; }

        public System.Data.Entity.DbSet<AJTaskManagerServiceService.DataObjects.TaskSubitemWork> TaskSubitemWorks { get; set; }
    }
}
