namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Supplier_SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "Supplier_SupplierId" });
            DropColumn("dbo.Products", "SupplierId");
            RenameColumn(table: "dbo.Products", name: "Supplier_SupplierId", newName: "SupplierId");
            DropPrimaryKey("dbo.Suppliers");
            AlterColumn("dbo.Products", "SupplierId", c => c.Long(nullable: false));
            AlterColumn("dbo.Products", "SupplierId", c => c.Long(nullable: false));
            AlterColumn("dbo.Suppliers", "SupplierId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Suppliers", "SupplierId");
            CreateIndex("dbo.Products", "SupplierId");
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropPrimaryKey("dbo.Suppliers");
            AlterColumn("dbo.Suppliers", "SupplierId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Products", "SupplierId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "SupplierId", c => c.Long());
            AddPrimaryKey("dbo.Suppliers", "SupplierId");
            RenameColumn(table: "dbo.Products", name: "SupplierId", newName: "Supplier_SupplierId");
            AddColumn("dbo.Products", "SupplierId", c => c.Long());
            CreateIndex("dbo.Products", "Supplier_SupplierId");
            AddForeignKey("dbo.Products", "Supplier_SupplierId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
        }
    }
}
