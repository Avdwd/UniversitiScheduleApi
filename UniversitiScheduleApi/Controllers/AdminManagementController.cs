using Microsoft.AspNetCore.Mvc;
using UNISchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;



namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminManagementController : ControllerBase
    {
        public readonly IAdminManagementService _adminManagementService;
        public AdminManagementController(IAdminManagementService adminManagementService)
        {
            _adminManagementService = adminManagementService;
        }
        // GET: /AdminManagement
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllAdmins()
        {
            var admins = await _adminManagementService.GetAllAdminsAsync();
            return Ok(admins);
        }
        // POST: /AdminManagement
        [HttpPost]
        public async Task<ActionResult<bool>> AddAdmin([FromBody] AdminRequest adminRequest)
        {
            var result = await _adminManagementService.AddAdminAsync(adminRequest.Email, adminRequest.Password);
            if (result)
            {
                return Ok(true);
            }
            return BadRequest("Failed to add admin. User may already exist or other error occurred.");
        }
        // DELETE: /AdminManagement/{email} 
        [HttpDelete("{email}")]
        public async Task<ActionResult<bool>> RemoveAdmin(string email)
        {
            var result = await _adminManagementService.RemoveAdminAsync(email);
            if (result)
            {
                return Ok(true);
            }
            return NotFound("Admin not found or could not be removed.");
        }
        // GET: /AdminManagement/Roles
        [HttpGet("Roles")]
        public async Task<ActionResult<List<RoleDtoResponse>>> GetAllRoles()
        {
            var roles = await _adminManagementService.GetAllRolesAsync();
            if (roles == null || !roles.Any())
            {
                return NotFound("No roles found.");
            }
            // Convert RoleDto to RoleDtoResponse
            var roleResponses = roles.Select(role => new RoleDtoResponse(
                role.Id,
                role.Name
            ));
            return Ok(roleResponses);
        }
        // GET: /AdminManagement/Users
        [HttpGet("Users")]
        public async Task<ActionResult<List<UserDtoResponse>>> GetAllUsers()
        {
            var users = await _adminManagementService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            // Convert UserDto to UserDtoResponse
            var userResponses = users.Select(user => new UserDtoResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Patronymic,
                user.Email
   
            ));
            return Ok(userResponses);
        }
        // GET: /AdminManagement/Users/{userId}
        [HttpGet("Users/{userId}")]
        public async Task<ActionResult<UserDtoResponse>> GetUserById(string userId)
        {
            var user = await _adminManagementService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            // Convert UserDto to UserDtoResponse
            var userResponse = new UserDtoResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Patronymic,
                user.Email
            );
            return Ok(userResponse);
        }
        // POST: /AdminManagement/Users
        [HttpPost("Users")]
        public async Task<ActionResult<(bool Succeeded, string[] Errors, string UserId)>> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            var (succeeded, errors, userId) = await _adminManagementService.CreateUserAsync(
                createUserRequest.Email,
                createUserRequest.Password,
                createUserRequest.FirstName,
                createUserRequest.LastName,
                createUserRequest.Patronymic
            );
            if (succeeded)
            {
                return Ok(userId);
            }
            return BadRequest(errors);
        }
        // PUT: /AdminManagement/Users/{userId}
        [HttpPut("Users/{userId}")]
        public async Task<ActionResult<(bool Succeeded, string[] Errors)>> UpdateUser(string userId, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var (succeeded, errors) = await _adminManagementService.UpdateUserAsync(
                userId,
                updateUserRequest.FirstName,
                updateUserRequest.LastName,
                updateUserRequest.Patronymic
            );
            if (succeeded)
            {
                return Ok();
            }
            return BadRequest(errors);
        }
        // DELETE: /AdminManagement/Users/{userId}
        [HttpDelete("Users/{userId}")]
        public async Task<ActionResult<bool>> DeleteUser(string userId)
        {
            var result = await _adminManagementService.DeleteUserAsync(userId);
            if (result.Succeeded)
            {
                return Ok(true);
            }
            return BadRequest(result.Errors);
        }
        // POST: /AdminManagement/Users/{userId}/Roles/{roleName}
        [HttpPost("Users/{userId}/Roles/{roleName}")]
        public async Task<ActionResult<(bool Succeeded, string[] Errors)>> AddUserToRole(string userId, string roleName)
        {
            var (succeeded, errors) = await _adminManagementService.AddUserToRoleAsync(userId, roleName);
            if (succeeded)
            {
                return Ok(userId);
            }
            return BadRequest(errors);
        }
        // DELETE: /AdminManagement/Users/{userId}/Roles/{roleName}
        [HttpDelete("Users/{userId}/Roles/{roleName}")]
        public async Task<ActionResult<(bool Succeeded, string[] Errors)>> RemoveUserFromRole(string userId, string roleName)
        {
            var (succeeded, errors) = await _adminManagementService.RemoveUserFromRoleAsync(userId, roleName);
            if (succeeded)
            {
                return Ok();
            }
            return BadRequest(errors);

        }
        // GET: /AdminManagement/Users/{userId}/Roles
        [HttpGet("Users/{userId}/Roles")]
        public async Task<ActionResult<IList<string>>> GetUserRoles(string userId)
        {
            var roles = await _adminManagementService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
        
    }

}
