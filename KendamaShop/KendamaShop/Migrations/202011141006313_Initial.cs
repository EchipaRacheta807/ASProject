namespace KendamaShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        SellerId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Price = c.Single(nullable: false),
                        Rating = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
