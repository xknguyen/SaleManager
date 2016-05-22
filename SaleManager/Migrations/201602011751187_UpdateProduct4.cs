namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderDetails", "Note", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderDetails", "Note", c => c.Long(nullable: false));
        }
    }
}
