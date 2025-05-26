using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IAdminManagementService
    {
        Task<bool> AddAdminAsync(string email, string password);
        Task<(bool Succeeded, string[] Errors)> AddUserToRoleAsync(string userId, string roleName);
        Task<(bool Succeeded, string[] Errors, string UserId)> CreateUserAsync(string email, string password, string firstName, string lastName, string patronymic);
        Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(string userId);
        Task<IEnumerable<string>> GetAllAdminsAsync();
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<bool> RemoveAdminAsync(string email);
        Task<(bool Succeeded, string[] Errors)> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(string userId, string firstName, string lastName, string patronymic);
    }
}