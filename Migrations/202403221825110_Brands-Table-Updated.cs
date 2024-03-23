namespace eticaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandsTableUpdated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Brands", "NumberOfProducts");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Brands", "NumberOfProducts", c => c.Int(nullable: false));
        }
    }
}
