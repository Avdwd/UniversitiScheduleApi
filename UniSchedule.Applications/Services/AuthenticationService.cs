using Microsoft.AspNetCore.Identity;

using System.Threading.Tasks;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Constants;
using UNISchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.Applications.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Якщо ви використовуєте ролі

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe = false)
        {
            // Це саме те, що робить Identity Login.cshtml.cs
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            return result; // Повертає результат входу
        }

        public async Task<IdentityResult> RegisterStudentAsync(string email, string password, string firstName, string lastName, string patronymic, Group group)
        {
            var user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName, LastName = lastName, Patronymic = patronymic };
            var result = await _userManager.CreateAsync(user, password);
            //Тут треба створити студента 
            // і призначити його до групи, якщо це потрібно
            var studentDetails = UserDetails.Create(user.Id, user.UserName, user.LastName, user.FirstName, user.Patronymic).userDatails;

            var studentProfile = StudentProfile.Create(
                user.Id,
                group,
                studentDetails
                );
            

            if (result.Succeeded)
            {
                // Призначення ролі "Student"
                if (!await _roleManager.RoleExistsAsync("Student"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Student"));
                }
                await _userManager.AddToRoleAsync(user, "Student");
            }
            return result;
        }

        public async Task<IdentityResult> RegisterTeacherAsync(string email, string password, string firstName, string lastName, string patronymic, Institute institute)
        {
            var user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName, LastName = lastName, Patronymic = patronymic };
            var result = await _userManager.CreateAsync(user, password);

            // Тут треба створити профіль викладача
            var teacherDetails = UserDetails.Create(user.Id, user.UserName, user.LastName, user.FirstName, user.Patronymic).userDatails;
            var teacherProfile = TeacherProfile.Create(
                user.Id,
                institute,
                teacherDetails
            );

            if (result.Succeeded)
            {
                // Призначення ролі "Teacher"
                if (!await _roleManager.RoleExistsAsync("Teacher"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Teacher"));
                }
                await _userManager.AddToRoleAsync(user, "Teacher");
            }
            return result;
        }

        public async Task LogoutAsync()
        {
            // Це те, що робить Identity Logout.cshtml.cs
            await _signInManager.SignOutAsync(); // Видаляє автентифікаційний файл cookie
        }
    }
}