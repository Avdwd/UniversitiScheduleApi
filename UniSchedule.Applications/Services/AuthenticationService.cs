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
        private readonly IStudentProfileService _studentProfileService;
        private readonly IAdminManagmentService _adminManagmentService;
        private readonly ITeacherProfileService _teacherProfileService;
        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStudentProfileService studentProfileService,
            IAdminManagmentService adminManagmentService,
            ITeacherProfileService teacherProfileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _studentProfileService = studentProfileService;
            _adminManagmentService = adminManagmentService;
            _teacherProfileService = teacherProfileService;
        }

        public async Task<SignInResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
        }
        //Method for register student
        public async Task<IdentityResult> RegisterStudentAsync(string email, string password, string lastName, string firstName, string patronymic, Group group)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                LastName = lastName,
                FirstName = firstName,
                Patronymic = patronymic,

            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return result;
            await _userManager.AddToRoleAsync(user, AppRoles.Student);

            var userDatails = UserDetails.Create(user.Id, user.UserName, user.LastName, user.FirstName, user.Patronymic).userDatails;
            var studentProfile = StudentProfile.Create(user.Id, group, userDatails).student;
            await _studentProfileService.CreateStudentProfile(studentProfile);

            return result;
        }
        //Method for register teacher
        public async Task<IdentityResult> RegisterTeacherAsync(string email, string password, string lastName, string firstName, string patronymic, Institute institute)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                LastName = lastName,
                FirstName = firstName,
                Patronymic = patronymic,
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return result;
            await _userManager.AddToRoleAsync(user, AppRoles.Teacher);
            var userDatails = UserDetails.Create(user.Id, user.UserName, user.LastName, user.FirstName, user.Patronymic).userDatails;
            var teacherProfile = TeacherProfile.Create(user.Id, institute, userDatails).teacher;
            await _teacherProfileService.CreateTeacherProfile(teacherProfile);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
