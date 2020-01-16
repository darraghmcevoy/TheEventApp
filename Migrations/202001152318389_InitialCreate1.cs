namespace TheEventApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Events", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Title", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Title", c => c.String());
            DropColumn("dbo.Events", "Mobile");
            DropColumn("dbo.Events", "Email");
        }
    }
}
