namespace DevLunch.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "UserName");
        }
    }
}
