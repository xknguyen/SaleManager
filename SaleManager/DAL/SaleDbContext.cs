using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaleManager.Models;

namespace SaleManager.DAL
{
    public class SaleDbContext : IdentityDbContext
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Thiết lập tên cho các bảng lưu trữ thông tin người dùng và quyền
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<IdentityUser>().ToTable("Accounts");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserInRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            
        }

        public SaleDbContext()
            : base("DefaultConnection")
        {
        }

        public static SaleDbContext Create()
        {
            return new SaleDbContext();
        }

    }
}