using SaleManager.DAL;

namespace SaleManager.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SaleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationsEnabled = false;
            ContextKey = "SaleManager.DAL.SaleDbContext";
        }

        protected override void Seed(SaleDbContext context)
        {
            AccountSeeder.Seed(context);
        }
    }
}
