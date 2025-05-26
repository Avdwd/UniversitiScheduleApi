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
        //ToDd: rewrite singn in result 

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStudentProfileService _studentProfileService;
        private readonly IAdminManagementService _adminManagmentService;
        private readonly ITeacherProfileService _teacherProfileService;
        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStudentProfileService studentProfileService,
            IAdminManagementService adminManagmentService,
            ITeacherProfileService teacherProfileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _studentProfileService = studentProfileService;
            _adminManagmentService = adminManagmentService;
            _teacherProfileService = teacherProfileService;
        }
        public async Task<LoginResultDto> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: true);

            return new LoginResultDto(
                result.Succeeded,
                result.IsLockedOut,
                result.IsNotAllowed,
                result.RequiresTwoFactor
            );
        }


        //Method for register student
        public async Task<RegisterResultDto> RegisterStudentAsync(string email, string password, string lastName, string firstName, string patronymic, Group group)
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

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, AppRoles.Student);

                var userDatails = UserDetails.Create(user.Id, user.UserName, user.LastName, user.FirstName, user.Patronymic).userDatails;
                var studentProfile = StudentProfile.Create(user.Id, group, userDatails).student;
                await _studentProfileService.CreateStudentProfile(studentProfile);
                return new RegisterResultDto(true, null, user.Id);
            }
            return new RegisterResultDto(false, result.Errors.Select(e => e.Description));
        }


        //Method for register teacher
        public async Task<RegisterResultDto> RegisterTeacherAsync(string email, string password, string lastName, string firstName, string patronymic, Institute institute)
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
            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, AppRoles.Teacher);
                var userDatails = UserDetails.Create(user.Id, user.UserName, user.LastName, user.FirstName, user.Patronymic).userDatails;
                var teacherProfile = TeacherProfile.Create(user.Id, institute, userDatails).teacher;
                await _teacherProfileService.CreateTeacherProfile(teacherProfile);
                return new RegisterResultDto(true, null, user.Id);
            }
            return new RegisterResultDto(false, result.Errors.Select(e => e.Description));
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
