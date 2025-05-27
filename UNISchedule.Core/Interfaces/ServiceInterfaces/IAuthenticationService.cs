using Microsoft.AspNetCore.Identity;
using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.ServiceInterfaces
{
    
        public interface IAuthenticationService
        {
            // Для входу:
            // Повертає Succeeded, RequiresTwoFactor, IsLockedOut
            // Не повертає токен, а встановлює cookie.
            Task<SignInResult> LoginAsync(string email, string password, bool rememberMe = false);

            // Для реєстрації (має повертати IdentityResult)
            Task<IdentityResult> RegisterStudentAsync(string email, string password, string firstName, string lastName, string patronymic, Group group);
            Task<IdentityResult> RegisterTeacherAsync(string email, string password, string firstName, string lastName, string patronymic, Institute institute);

            // Для виходу:
            // Видаляє cookie
            Task LogoutAsync();
        }
    
}