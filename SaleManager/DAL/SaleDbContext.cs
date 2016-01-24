using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaleManager.Models;

namespace SaleManager.DAL
{
    public class SaleDbContext : IdentityDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails{ get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers{ get; set; }
        public DbSet<SystemParameter> SystemParameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Thiết lập tên cho các bảng lưu trữ thông tin người dùng và quyền
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Accounts");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserInRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");

            // Thiết lập mối quan hệ giữa nhóm con và nhóm cha
            modelBuilder.Entity<Category>()
                .HasMany(c => c.ChildCategories)
                .WithOptional(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithRequired(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .WillCascadeOnDelete(true);

            // Thiết lập mối quan hệ giữa Nhà cung cấp và sản phẩm
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithRequired(p => p.Supplier)
                .WillCascadeOnDelete(true);


            // Thiết lập mối quan hệ giữa đơn hàng và Chi tiết đơn hàng
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithRequired(d => d.Order)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Product>()
                .HasMany(o => o.OrderDetails)
                .WithRequired(d => d.Product)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Product>()
            //    .HasMany(o => o.OrderDetails)
            //    .WithRequired(d => d.Product)
            //    .HasForeignKey(c => c.ProductId)
            //    .WillCascadeOnDelete(true);

            modelBuilder.Entity<Customer>()
                .HasMany(o => o.Orders)
                .WithRequired(d => d.Customer)
                .HasForeignKey(c => c.CustomerId)
                .WillCascadeOnDelete(true);
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