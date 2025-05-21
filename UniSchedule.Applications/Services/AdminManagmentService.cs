using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.Applications.Services
{
    public class AdminManagmentService : IAdminManagmentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminManagmentService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddAdminAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return false; // Користувач уже існує

            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return false;

            // Перевірка, чи існує роль Admin, і створення її, якщо потрібно
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            return true;
        }

        public async Task<bool> RemoveAdminAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }

            // Якщо потрібно повністю видалити користувача:
            //await _userManager.DeleteAsync(user);

            return true;
        }


        // Управління користувачами Identity (адміністративні функції)
        public async Task<(bool Succeeded, string[] Errors, string UserId)> CreateUserAsync(CreateUserRequest request) {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Patronymic = request.Patronymic
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return (true, null, user.Id);
            }
            return (false, result.Errors.Select(e => e.Description).ToArray(), null);
        } 

        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(string userId, UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Patronymic = request.Patronymic;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return (true, null);
            }
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }
        public async Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return (true, null);
            }
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }



    }
}
