namespace eticaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Detail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Detail", c => c.String());
        }
    }
}
