namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierId" });
            AddColumn("dbo.Products", "Supplier_SupplierId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "SupplierId", c => c.Long());
            CreateIndex("dbo.Products", "Supplier_SupplierId");
            AddForeignKey("dbo.Products", "Supplier_SupplierId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Supplier_SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "Supplier_SupplierId" });
            AlterColumn("dbo.Products", "SupplierId", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "Supplier_SupplierId");
            CreateIndex("dbo.Products", "SupplierId");
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
        }
    }
}
