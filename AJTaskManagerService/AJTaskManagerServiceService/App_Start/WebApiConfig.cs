using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using AJTaskManagerServiceService.DataObjects;
using AJTaskManagerServiceService.Migrations;
using AJTaskManagerServiceService.Models;

namespace AJTaskManagerServiceService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Set default and null value handling to "Include" for Json Serializer
            config.Formatters.JsonFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;

            //Database.SetInitializer(new AJTaskManagerServiceInitializer());
            var migrator = new DbMigrator(new Configuration());
            migrator.Update();
        }
    }

    public class AJTaskManagerServiceInitializer : ClearDatabaseSchemaIfModelChanges<AJTaskManagerServiceContext>
    {
        protected override void Seed(AJTaskManagerServiceContext context)
        {
            //List<TodoItem> todoItems = new List<TodoItem>
            //{
            //    new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
            //    new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            //};

            //foreach (TodoItem todoItem in todoItems)
            //{
            //    context.Set<TodoItem>().Add(todoItem);
            //}

            //context.Set<User>().Add(new User());

            //context.Set<Group>().Add(new Group() { GroupName = "TestGroup" });

            //context.Set<Role>();
            //context.Set<UserGroupRole>();

            //context.Set<UserDomain>().Add(new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Microsoft", DomainKey = 1 });
            //context.Set<UserDomain>().Add(new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Facebook", DomainKey = 2 });
            //context.Set<UserDomain>().Add(new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Google", DomainKey = 3 });
            //context.Set<UserDomain>().Add(new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Twitter", DomainKey = 4 });

            //context.SaveChanges();
            //base.Seed(context);

            //context.Set<RoleType>().Add(new RoleType() { Id = Guid.NewGuid().ToString(), RoleName = "Admin", RoleKey = 1 });
            //context.Set<RoleType>().Add(new RoleType() { Id = Guid.NewGuid().ToString(), RoleName = "Editor", RoleKey = 2 });
            //context.Set<RoleType>().Add(new RoleType() { Id = Guid.NewGuid().ToString(), RoleName = "Viewer", RoleKey = 3 });

            //context.SaveChanges();
            //base.Seed(context);

            //context.Set<TaskStatus>().Add(new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "Not Started" });
            //context.Set<TaskStatus>().Add(new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "In Progress" });
            //context.Set<TaskStatus>().Add(new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "Completed" });
            //context.Set<TaskStatus>().Add(new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "Rejected" });

            //context.SaveChanges();
            //base.Seed(context);

            context.TaskStatus.AddOrUpdate(t => t.Id,
                new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "Not Started" },
                new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "In Progress" },
                new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "Completed" },
                new TaskStatus() { Id = Guid.NewGuid().ToString(), Status = "Rejected" }
                );

            context.RoleTypes.AddOrUpdate(t => t.Id,
               new RoleType() { Id = Guid.NewGuid().ToString(), RoleName = "Admin", RoleKey = 1 },
               new RoleType() { Id = Guid.NewGuid().ToString(), RoleName = "Editor", RoleKey = 2 },
               new RoleType() { Id = Guid.NewGuid().ToString(), RoleName = "Viewer", RoleKey = 3 }
               );

            context.UserDomains.AddOrUpdate(t => t.Id,
              new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Microsoft", DomainKey = 1 },
              new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Facebook", DomainKey = 2 },
              new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Google", DomainKey = 3 },
              new UserDomain() { Id = Guid.NewGuid().ToString(), UserDomainName = "Twitter", DomainKey = 4 }
              );

            context.SaveChanges();
            base.Seed(context);
        }
    }
}

