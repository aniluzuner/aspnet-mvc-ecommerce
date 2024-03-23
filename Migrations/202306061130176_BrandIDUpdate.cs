namespace eticaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandIDUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "BrandID", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "Brand");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Brand", c => c.String(nullable: false));
            DropColumn("dbo.Products", "BrandID");
        }
    }
}
