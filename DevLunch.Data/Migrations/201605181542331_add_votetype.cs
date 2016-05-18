namespace DevLunch.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_votetype : DbMigration
    {
        public override void Up()
        {
            this.Sql("TRUNCATE TABLE dbo.Votes");
            AddColumn("dbo.Votes", "VoteType", c => c.Int(nullable: false));
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "VoteType");
        }
    }
}
