namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSupplier : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Suppliers", "Actived");
            DropColumn("dbo.Suppliers", "RowVersion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Suppliers", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Suppliers", "Actived", c => c.Boolean(nullable: false));
        }
    }
}
