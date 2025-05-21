using Microsoft.AspNetCore.Identity;
using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        Task<SignInResult> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<IdentityResult> RegisterStudentAsync(string email, string password, string lastName, string firstName, string patronymic, Group group);
        Task<IdentityResult> RegisterTeacherAsync(string email, string password, string lastName, string firstName, string patronymic, Institute institute);
    }
}