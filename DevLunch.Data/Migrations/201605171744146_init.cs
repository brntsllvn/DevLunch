namespace DevLunch.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lunches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Host = c.String(),
                        MeetingTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Lunch_Id = c.Int(),
                        Restaurant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Lunches", t => t.Lunch_Id)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_Id)
                .Index(t => t.Lunch_Id)
                .Index(t => t.Restaurant_Id);
            
            CreateTable(
                "dbo.RestaurantLunches",
                c => new
                    {
                        Restaurant_Id = c.Int(nullable: false),
                        Lunch_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Restaurant_Id, t.Lunch_Id })
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_Id, cascadeDelete: true)
                .ForeignKey("dbo.Lunches", t => t.Lunch_Id, cascadeDelete: true)
                .Index(t => t.Restaurant_Id)
                .Index(t => t.Lunch_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "Restaurant_Id", "dbo.Restaurants");
            DropForeignKey("dbo.Votes", "Lunch_Id", "dbo.Lunches");
            DropForeignKey("dbo.RestaurantLunches", "Lunch_Id", "dbo.Lunches");
            DropForeignKey("dbo.RestaurantLunches", "Restaurant_Id", "dbo.Restaurants");
            DropIndex("dbo.RestaurantLunches", new[] { "Lunch_Id" });
            DropIndex("dbo.RestaurantLunches", new[] { "Restaurant_Id" });
            DropIndex("dbo.Votes", new[] { "Restaurant_Id" });
            DropIndex("dbo.Votes", new[] { "Lunch_Id" });
            DropTable("dbo.RestaurantLunches");
            DropTable("dbo.Votes");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Lunches");
        }
    }
}
