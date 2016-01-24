namespace SaleManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemID : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemParameters",
                c => new
                    {
                        SystemParameterId = c.Long(nullable: false, identity: true),
                        SystemValue = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SystemParameterId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemParameters");
        }
    }
}
