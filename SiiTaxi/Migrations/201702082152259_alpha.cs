namespace SiiTaxi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alpha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Taxis", "bigTaxi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Taxis", "bigTaxi");
        }
    }
}
