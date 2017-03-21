namespace AJTaskManagerServiceService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsUserDefaultGroupColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("AJTaskManagerService.UserGroups", "IsUserDefaultGroup", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("AJTaskManagerService.UserGroups", "IsUserDefaultGroup");
        }
    }
}
