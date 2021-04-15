namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbchanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "RecipientID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "RecipientID", c => c.Int(nullable: false));
        }
    }
}
