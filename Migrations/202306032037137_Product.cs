namespace eticaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "imageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "imageUrl", c => c.Int(nullable: true));
        }
    }
}
