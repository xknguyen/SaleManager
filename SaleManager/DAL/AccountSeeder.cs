using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaleManager.Models;

namespace SaleManager.DAL
{
    public class AccountSeeder
    {
        public static void Seed(SaleDbContext context)
        {
            // Tạo các đối tượng quản lý quyền và người dùng
            var userManager = new UserManager<Account>(new UserStore<Account>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string adminRole = "Admin",
                customerRole = "Customer",
                password = "123456";

            // Tạo các quyền (vai trò của người dùng trong hệ thống)
            if (!roleManager.RoleExists(adminRole))
                roleManager.Create(new IdentityRole(adminRole));

            if (!roleManager.RoleExists(customerRole))
                roleManager.Create(new IdentityRole(customerRole));

            // Tạo tài khoản Admin
            var adminUser = new Account()
            {
                UserName = "admin",
                Profile = new UserProfile()
                {
                    LastName = "Nguyễn Quốc",
                    FirstName = "Anh",
                    BirthDate = new DateTime(1992, 1, 11)
                }
            };
            var result = userManager.Create(adminUser, password);
            
            // Gán quyền Admin và Teacher cho người dùng vừa tạo
            if (result.Succeeded)
            {
                userManager.AddToRole(adminUser.Id, adminRole);
                userManager.AddToRole(adminUser.Id, customerRole);
            }
        }
    }
}