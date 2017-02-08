namespace SiiTaxi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "Taxi_TaxiID1", "dbo.Taxis");
            DropIndex("dbo.People", new[] { "Taxi_TaxiID1" });
            AddColumn("dbo.People", "IsApprover", c => c.Boolean(nullable: false));
            DropColumn("dbo.People", "TaxiID");
            DropColumn("dbo.People", "Taxi_TaxiID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "Taxi_TaxiID1", c => c.Int());
            AddColumn("dbo.People", "TaxiID", c => c.Int(nullable: false));
            DropColumn("dbo.People", "IsApprover");
            CreateIndex("dbo.People", "Taxi_TaxiID1");
            AddForeignKey("dbo.People", "Taxi_TaxiID1", "dbo.Taxis", "TaxiID");
        }
    }
}
