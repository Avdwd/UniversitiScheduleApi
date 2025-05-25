using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UNISchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
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
                return false; // user exist

            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return false;

            // Is admin exist 
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

            return true;
        }

        // Managment users from Identity (Admin functions)
        public async Task<(bool Succeeded, string[] Errors, string UserId)> CreateUserAsync(
            string email, string password, string firstName, string lastName, string patronymic)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Patronymic = patronymic
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return (true, null, user.Id);
            }
            return (false, result.Errors.Select(e => e.Description).ToArray(), null);
        }

        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(
            string userId, string firstName, string lastName, string patronymic)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Patronymic = patronymic;
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

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            //Mapping ApplicationUser to UserDto
            return new UserDto
            (
                user.Id,
                user.Email,
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Patronymic
            );
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            //Mapping ApplicationUser to UserDto
            return users.Select(user => new UserDto(
                user.Id,
                user.Email,
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Patronymic
            )).ToList();
        }

        //Role Management

        public async Task<(bool Succeeded, string[] Errors)> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }


            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));//Створення на льоту ролі (потрібно буде прибрати )
                if (!roleResult.Succeeded)
                {
                    return (false, roleResult.Errors.Select(e => e.Description).ToArray());
                }
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return (true, null);
            }
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<(bool Succeeded, string[] Errors)> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                return (false, new[] { $"User is not in role '{roleName}'" });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return (true, null);
            }
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<string>(); // Повертаємо порожній список, якщо користувача не знайдено
            }
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            // Mapping IdentityRole to RoleDto
            return roles.Select(role => new RoleDto(
                role.Id,
                role.Name
            )).ToList();
        }
    }

}
