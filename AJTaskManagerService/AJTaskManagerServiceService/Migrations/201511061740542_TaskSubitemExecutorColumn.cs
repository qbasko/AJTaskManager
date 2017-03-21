namespace AJTaskManagerServiceService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskSubitemExecutorColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("AJTaskManagerService.TaskSubitems", "ExecutorId", c => c.String(maxLength: 128));
            CreateIndex("AJTaskManagerService.TaskSubitems", "ExecutorId");
            AddForeignKey("AJTaskManagerService.TaskSubitems", "ExecutorId", "AJTaskManagerService.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("AJTaskManagerService.TaskSubitems", "ExecutorId", "AJTaskManagerService.Users");
            DropIndex("AJTaskManagerService.TaskSubitems", new[] { "ExecutorId" });
            DropColumn("AJTaskManagerService.TaskSubitems", "ExecutorId");
        }
    }
}
