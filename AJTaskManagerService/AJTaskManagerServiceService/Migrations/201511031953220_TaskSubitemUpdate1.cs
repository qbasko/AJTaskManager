namespace AJTaskManagerServiceService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskSubitemUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("AJTaskManagerService.TaskSubitems", "IsCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("AJTaskManagerService.TaskSubitems", "IsCompleted");
        }
    }
}
