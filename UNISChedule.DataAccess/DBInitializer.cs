using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.DataAccess.Entities.Identity;
using UNISchedule.Core.Constants;

namespace UNISchedule.DataAccess
{
    public static class DBInitializer
    {
        public static async Task Initialize(
           UniScheduleDbContext context,
           UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            //Застосування міграцій 
            // Важливо! для продакшену розглянути інші стратегії застосування міграцій
            // (наприклад, окремий скрипт, CI/CD пайплайн).
            await context.Database.MigrateAsync();

           
            await SeedRolesAsync(roleManager);

            
            await SeedAdminUserAsync(userManager, roleManager);

            
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(AppRoles.Teacher))
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Teacher));
            }
            if (!await roleManager.RoleExistsAsync(AppRoles.Student))
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Student));
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@unishedule.com"); // Використати  реальну пошту
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@unishedule.com",
                    Email = "admin@unishedule.com",
                    EmailConfirmed = true,
                    FirstName = "Super",
                    LastName = "Admin"
                };
                var result = await userManager.CreateAsync(adminUser, "SuperSecure@123!"); // !!! Змінити на сильний пароль !!!
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
                }
                else
                {
                    // Обробка помилок створення адміністратора
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
