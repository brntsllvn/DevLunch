namespace DevLunch.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lunches", "DestinationRestaurant_Id", "dbo.Restaurants");
            DropIndex("dbo.Lunches", new[] { "Restaurant_Id" });
            DropColumn("dbo.Lunches", "Restaurant_Id");

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
            AddColumn("dbo.Lunches", "Restaurant_Id", c => c.Int());
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
            CreateIndex("dbo.Lunches", "Restaurant_Id");
            AddForeignKey("dbo.Lunches", "Restaurant_Id", "dbo.Restaurants", "Id");
        }
    }
}
