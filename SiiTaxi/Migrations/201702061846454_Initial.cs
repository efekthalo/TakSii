namespace SiiTaxi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        EmailAlt = c.String(),
                        Phone = c.String(),
                        TaxiID = c.Int(nullable: false),
                        Taxi_TaxiID = c.Int(),
                        Taxi_TaxiID1 = c.Int(),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("dbo.Taxis", t => t.Taxi_TaxiID)
                .ForeignKey("dbo.Taxis", t => t.Taxi_TaxiID1)
                .Index(t => t.Taxi_TaxiID)
                .Index(t => t.Taxi_TaxiID1);
            
            CreateTable(
                "dbo.Taxis",
                c => new
                    {
                        TaxiID = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(),
                        Date = c.DateTime(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ApproverID = c.Int(nullable: false),
                        ownerConfirmed = c.Boolean(nullable: false),
                        approverConfirmed = c.Boolean(nullable: false),
                        Approver_PersonID = c.Int(),
                        Owner_PersonID = c.Int(),
                    })
                .PrimaryKey(t => t.TaxiID)
                .ForeignKey("dbo.People", t => t.Approver_PersonID)
                .ForeignKey("dbo.People", t => t.Owner_PersonID)
                .Index(t => t.Approver_PersonID)
                .Index(t => t.Owner_PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "Taxi_TaxiID1", "dbo.Taxis");
            DropForeignKey("dbo.People", "Taxi_TaxiID", "dbo.Taxis");
            DropForeignKey("dbo.Taxis", "Owner_PersonID", "dbo.People");
            DropForeignKey("dbo.Taxis", "Approver_PersonID", "dbo.People");
            DropIndex("dbo.Taxis", new[] { "Owner_PersonID" });
            DropIndex("dbo.Taxis", new[] { "Approver_PersonID" });
            DropIndex("dbo.People", new[] { "Taxi_TaxiID1" });
            DropIndex("dbo.People", new[] { "Taxi_TaxiID" });
            DropTable("dbo.Taxis");
            DropTable("dbo.People");
        }
    }
}
