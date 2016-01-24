namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCustomerActived : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "Actived");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Actived", c => c.Boolean(nullable: false));
        }
    }
}
