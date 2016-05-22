namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Alias");
            DropColumn("dbo.Products", "RowVersion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Products", "Alias", c => c.String());
        }
    }
}
