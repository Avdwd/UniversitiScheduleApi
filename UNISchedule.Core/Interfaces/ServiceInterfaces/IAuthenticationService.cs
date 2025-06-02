using Microsoft.AspNetCore.Identity;

namespace UNISchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        Task<SignInResult> LoginAsync(string email, string password, bool rememberMe = false);
        Task LogoutAsync();
        Task<IdentityResult> RegisterStudentAsync(string email, string password, string firstName, string lastName, string patronymic);
        Task<IdentityResult> RegisterTeacherAsync(string email, string password, string firstName, string lastName, string patronymic);
    }
}