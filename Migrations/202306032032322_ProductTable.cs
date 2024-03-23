namespace eticaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Detail", c => c.String());
            
        }
        
        public override void Down()
        {

            DropColumn("dbo.Products", "Detail");
        }
    }
}
