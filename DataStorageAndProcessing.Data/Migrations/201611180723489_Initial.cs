namespace DataStorageAndProcessing.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Institutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .Index(t => t.LocationID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InstitutionRaitings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordRank = c.Int(nullable: false),
                        NationalRank = c.Int(nullable: false),
                        QualityofEducation = c.Int(nullable: false),
                        AlumniEmployment = c.Int(nullable: false),
                        QualityofFaculty = c.Int(nullable: false),
                        Publications = c.Int(nullable: false),
                        Influence = c.Int(nullable: false),
                        Citations = c.Int(nullable: false),
                        BroadImpact = c.Int(nullable: false),
                        Patents = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        RaitingID = c.Int(nullable: false),
                        InstitutionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionID, cascadeDelete: true)
                .ForeignKey("dbo.Raitings", t => t.RaitingID, cascadeDelete: true)
                .Index(t => t.RaitingID)
                .Index(t => t.InstitutionID);
            
            CreateTable(
                "dbo.Raitings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InstitutionRaitings", "RaitingID", "dbo.Raitings");
            DropForeignKey("dbo.InstitutionRaitings", "InstitutionID", "dbo.Institutions");
            DropForeignKey("dbo.Institutions", "LocationID", "dbo.Locations");
            DropIndex("dbo.InstitutionRaitings", new[] { "InstitutionID" });
            DropIndex("dbo.InstitutionRaitings", new[] { "RaitingID" });
            DropIndex("dbo.Institutions", new[] { "LocationID" });
            DropTable("dbo.Raitings");
            DropTable("dbo.InstitutionRaitings");
            DropTable("dbo.Locations");
            DropTable("dbo.Institutions");
        }
    }
}
