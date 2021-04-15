namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        GroupUserID = c.Int(nullable: false, identity: true),
                        Color = c.String(),
                        GroupID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GroupUserID)
                .ForeignKey("dbo.Groups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.GroupID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        MessageText = c.String(),
                        RecipientType = c.String(),
                        RecipientID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupUsers", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupUsers", "GroupID", "dbo.Groups");
            DropIndex("dbo.Messages", new[] { "UserID" });
            DropIndex("dbo.GroupUsers", new[] { "UserID" });
            DropIndex("dbo.GroupUsers", new[] { "GroupID" });
            DropTable("dbo.Messages");
            DropTable("dbo.GroupUsers");
            DropTable("dbo.Groups");
        }
    }
}
