namespace eticaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriesAndSubCategoriesTablesUpdated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Categories", "NumberOfProducts");
            DropColumn("dbo.SubCategories", "NumberOfProducts");
            DropColumn("dbo.SubCategories", "ParentCategoryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubCategories", "ParentCategoryName", c => c.String(nullable: false));
            AddColumn("dbo.SubCategories", "NumberOfProducts", c => c.Int(nullable: false));
            AddColumn("dbo.Categories", "NumberOfProducts", c => c.Int(nullable: false));
        }
    }
}
