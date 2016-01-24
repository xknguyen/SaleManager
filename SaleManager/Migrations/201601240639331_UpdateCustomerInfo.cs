namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCustomerInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropPrimaryKey("dbo.Customers");
            AddColumn("dbo.Customers", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "CustomerId", c => c.Long(nullable: false));
            AlterColumn("dbo.Customers", "CustomerId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Customers", "CustomerId");
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "CustomerId", cascadeDelete: true);
            DropColumn("dbo.Customers", "LastName");
            DropColumn("dbo.Customers", "FirstName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.Customers", "LastName", c => c.String(nullable: false));
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropPrimaryKey("dbo.Customers");
            AlterColumn("dbo.Customers", "CustomerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "CustomerId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Customers", "FullName");
            AddPrimaryKey("dbo.Customers", "CustomerId");
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "CustomerId", cascadeDelete: true);
        }
    }
}
