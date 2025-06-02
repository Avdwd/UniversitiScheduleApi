using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
// using UniSchedule.Core.Interfaces.ServiceInterfaces; // Можливо, це не потрібно, якщо IAuthenticationService у тому ж неймспейсі
using UNISchedule.Core.Constants; // Можливо, для ролей
using UNISchedule.Core.Interfaces.ServiceInterfaces;

// using UNISchedule.Core.Interfaces.ServiceInterfaces; // Залишити тільки один імпорт для IAuthenticationService, якщо їх два
using UNISchedule.Core.Models; // Для StudentProfile, TeacherProfile, UserDetails, Institute, Group - якщо вони використовуються як DTO
using UNISchedule.DataAccess.Entities.Identity; // Для ApplicationUser

namespace UNISchedule.Applications.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Тепер AuthenticationService не залежить від IStudentProfileService та ITeacherProfileService
        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe = false)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            return result;
        }

        public async Task<IdentityResult> RegisterStudentAsync(string email, string password, string firstName, string lastName, string patronymic)
        {
            var user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName, LastName = lastName, Patronymic = patronymic };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Призначення ролі "Student"
                if (!await _roleManager.RoleExistsAsync(AppRoles.Student)) 
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.Student));
                }
                await _userManager.AddToRoleAsync(user, AppRoles.Student);

               
            }
            return result;
        }

        public async Task<IdentityResult> RegisterTeacherAsync(string email, string password, string firstName, string lastName, string patronymic)
        {
            var user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName, LastName = lastName, Patronymic = patronymic };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Призначення ролі "Teacher"
                if (!await _roleManager.RoleExistsAsync(AppRoles.Teacher)) 
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.Teacher));
                }
                await _userManager.AddToRoleAsync(user, AppRoles.Teacher);

                
            }
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}