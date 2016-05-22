namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "SupplierId" });
            AlterColumn("dbo.Products", "SupplierId", c => c.Long());
            CreateIndex("dbo.Products", "SupplierId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "SupplierId" });
            AlterColumn("dbo.Products", "SupplierId", c => c.Long(nullable: false));
            CreateIndex("dbo.Products", "SupplierId");
        }
    }
}
