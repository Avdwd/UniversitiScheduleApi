using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities.Identity;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherProfileController : ControllerBase
    {
        public readonly ITeacherProfileService _teacherProfileService;
        public readonly IInstituteService _instituteService;
        private readonly UserManager<ApplicationUser> _userManager;
        public TeacherProfileController(ITeacherProfileService teacherProfileService, IInstituteService instituteService, UserManager<ApplicationUser> userManager)
        {
            _teacherProfileService = teacherProfileService;
            _instituteService = instituteService;
            _userManager = userManager;
        }
        // GET: /TeacherProfile/{teacherId}
        [HttpGet("{teacherId}")]
        public async Task<ActionResult<TeacherProfileResponse>> GetTeacherProfileById(string teacherId)
        {
            var teacherProfile = await _teacherProfileService.GetTeacherProfileById(teacherId);
            if (teacherProfile == null)
            {
                return NotFound();
            }
            var teacherProfileResponse = new TeacherProfileResponse(
                teacherProfile.ApplicationUserId,
                teacherProfile.UserDetails.FirstName,
                teacherProfile.UserDetails.LastName,
                teacherProfile.UserDetails.Patronymic,
                teacherProfile.Institute.Id
            );
            return Ok(teacherProfileResponse);
        }
        // GET: /TeacherProfile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherProfileResponse>>> GetAllTeacherProfiles()
        {
            var teacherProfiles = await _teacherProfileService.GetAllTeacherProfiles();
            if (teacherProfiles == null || !teacherProfiles.Any())
            {
                return Ok(new List<TeacherProfileResponse>());
            }
            var teacherProfileResponses = teacherProfiles.Select(tp => new TeacherProfileResponse(
                tp.ApplicationUserId,
                tp.UserDetails.FirstName,
                tp.UserDetails.LastName,
                tp.UserDetails.Patronymic,
                tp.Institute.Id
            ));
            return Ok(teacherProfileResponses);
        }
        // POST: /TeacherProfile
        [HttpPost]
        public async Task<ActionResult<string>> CreateTeacherProfile([FromBody] TeacherProfileRequest teacherProfile)
        {
            if (teacherProfile == null)
            {
                return BadRequest("Invalid teacher profile data.");
            }
            var institute = await _instituteService.GetInstituteById(teacherProfile.Institute.Id);

            ApplicationUser userToLink;
            var existingUser = await _userManager.FindByNameAsync(teacherProfile.UserName);
            if (existingUser == null)
            {
                // Якщо користувача не існує, створюємо нового користувача Identity
                var newUser = new ApplicationUser
                {
                    UserName = teacherProfile.UserName,
                    Email = teacherProfile.UserName + "@university.com" // Можливо, вам потрібно додати поле Email до TeacherProfileRequest
                };

                // !!! ВАЖЛИВО: Для реального використання пароль не має бути фіксованим рядком!
                // Він має надходити з запиту клієнта або генеруватися безпечним способом.
                // Наприклад, ви можете додати поле "Password" до TeacherProfileRequest.
                var createResult = await _userManager.CreateAsync(newUser, "SecureP@ssword123!"); // ЗМІНІТЬ ЦЕ!

                if (!createResult.Succeeded)
                {
                    // Повертаємо помилки, якщо створення користувача Identity не вдалося
                    return BadRequest($"Failed to create Identity user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }
                userToLink = newUser; // Використовуємо щойно створеного користувача
            }
            else
            {
                userToLink = existingUser; // Використовуємо існуючого користувача
            }
            var newTeacherProfileId = Guid.NewGuid().ToString();

            var (teacher, error) = TeacherProfile.Create(
                newTeacherProfileId,      // ID для TeacherProfile
                userToLink.Id,            // ID пов'язаного ApplicationUser (з БД)
                institute,
               UserDetails.Create(userToLink.Id, teacherProfile.UserName, teacherProfile.LastName, teacherProfile.FirstName, teacherProfile.MiddleName).userDatails
            );
            var teacherProfileId = await _teacherProfileService.CreateTeacherProfile(teacher);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            return Ok(teacherProfileId);
        }

        // PUT: /TeacherProfile/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateTeacherProfile(string id, [FromBody] TeacherProfileRequest teacherProfile)
        {
            if (teacherProfile == null ||  id== null)
            {
                return BadRequest("Invalid teacher profile data.");
            }
            if (string.IsNullOrEmpty(teacherProfile.FirstName) || string.IsNullOrEmpty(teacherProfile.LastName))
            {
                return BadRequest("First name and last name cannot be empty.");
            }
            var updatedProfileId = await _teacherProfileService.UpdateTeacherProfile(id, teacherProfile.FirstName, teacherProfile.LastName, teacherProfile.MiddleName, teacherProfile.Institute.Id);

            return Ok(updatedProfileId);
        }
        // DELETE: /TeacherProfile/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteTeacherProfile(string id)
        {
            var deleted = await _teacherProfileService.DeleteTeacherProfile(id);
            if (string.IsNullOrEmpty(deleted))
            {
                return NotFound();
            }
            return Ok(id);
        }

        // GET: /TeacherProfile/Institute/{instituteId}
        [HttpGet("Institute/{instituteId}")]
        public async Task<ActionResult<IEnumerable<TeacherProfileResponse>>> GetTeachersByInstitute(Guid instituteId)
        {
            var teacherProfiles = await _teacherProfileService.GetTeacherProfilesByInstitute(instituteId);
            if (teacherProfiles == null || !teacherProfiles.Any())
            {
                return Ok(new List<TeacherProfileResponse>());
            }
            var teacherProfileResponses = teacherProfiles.Select(tp => new TeacherProfileResponse(
                tp.ApplicationUserId,
                tp.UserDetails.FirstName,
                tp.UserDetails.LastName,
                tp.UserDetails.Patronymic,
                tp.Institute.Id
            ));
            return Ok(teacherProfileResponses);

        }
        // GET: /TeacherProfile/UserDatails{userDatails}
        [HttpGet("UserDetails/{userId}")]
        public async Task<ActionResult<TeacherProfileResponse>> GetTeacherProfileByUserId([FromBody] UserDetailsRequest userDetailsRequest)
        {
            if (userDetailsRequest == null || string.IsNullOrEmpty(userDetailsRequest.Id))
            {
                return BadRequest("Invalid user details.");
            }
            var userDetails = UserDetails.Create(userDetailsRequest.Id, userDetailsRequest.UserName, userDetailsRequest.LastName, userDetailsRequest.FirstName, userDetailsRequest.MiddleName).userDatails;
            var teacherProfile = await _teacherProfileService.GetTeacherProfileByUserDetails(userDetails);
            if (teacherProfile == null)
            {
                return NotFound();
            }
            var teacherProfileResponse = new TeacherProfileResponse(
                teacherProfile.ApplicationUserId,
                teacherProfile.UserDetails.FirstName,
                teacherProfile.UserDetails.LastName,
                teacherProfile.UserDetails.Patronymic,
                teacherProfile.Institute.Id
            );
            return Ok(teacherProfileResponse);
        }

        // GET: /TeacherProfiles/Page/{pageNumber}/{pageSize}
        [HttpGet("Page/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<TeacherProfileResponse>>> GetTeacherProfiles(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            if (pageSize > 100)
            {
                return BadRequest("Page size cannot exceed 100.");
            }

            var teacherProfiles = await _teacherProfileService.GetTeacherProfiles(pageNumber, pageSize);
            if (teacherProfiles == null || !teacherProfiles.Any())
            {
                return Ok(new List<TeacherProfileResponse>());
            }
            var teacherProfileResponses = teacherProfiles.Select(tp => new TeacherProfileResponse(
                tp.ApplicationUserId,
                tp.UserDetails.FirstName,
                tp.UserDetails.LastName,
                tp.UserDetails.Patronymic,
                tp.Institute.Id
            ));
            return Ok(teacherProfileResponses);

        }
    }
}
