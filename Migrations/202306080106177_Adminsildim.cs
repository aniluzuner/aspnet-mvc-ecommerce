namespace eticaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adminsildim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sellers", "isApproved", c => c.Boolean(nullable: false));
            DropTable("dbo.Admins");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Sellers", "isApproved");
        }
    }
}
