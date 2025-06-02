using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities.Identity;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentProfileController : ControllerBase
    {
        public readonly IStudentProfileService _studentProfileService;
        private readonly IGroupService _groupService;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentProfileController(IStudentProfileService studentProfileService, IGroupService groupService, UserManager<ApplicationUser> userManager)
        {
            _studentProfileService = studentProfileService;
            _groupService = groupService;
            _userManager = userManager;
        }

        // GET: /StudentProfile/{studentId}
        [HttpGet("{studentId}")]
        public async Task<ActionResult<StudentProfileResponse>> GetStudentProfileById(string studentId)
        {
            var studentProfile = await _studentProfileService.GetStudentProfileById(studentId);
            if (studentProfile == null)
            {
                return NotFound();
            }
            var studentProfileResponse =  new StudentProfileResponse(
                    studentProfile.ApplicationUserId,
                    studentProfile.UserDetails.FirstName,
                    studentProfile.UserDetails.LastName,
                    studentProfile.UserDetails.Patronymic,
                    studentProfile.Group.Institute.Id,
                    studentProfile.Group.Id
                );
            
            return Ok(studentProfileResponse);
        }
        // GET: /StudentProfileAll
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentProfileResponse>>> GetAllStudentProfiles()
        {
            var studentProfiles = await _studentProfileService.GetAllStudentProfiles();
            if (studentProfiles == null || !studentProfiles.Any())
            {
                return NotFound();
            }
            var studentProfileResponses = studentProfiles.Select(sp => new StudentProfileResponse(
                sp.ApplicationUserId,
                sp.UserDetails.FirstName,
                sp.UserDetails.LastName,
                sp.UserDetails.Patronymic,
                sp.Group.Institute.Id,
                sp.Group.Id
            ));
            return Ok(studentProfileResponses);
        }
        // POST: /StudentProfileCreate
        [HttpPost]
        public async Task<ActionResult<string>> CreateStudentProfile([FromBody] StudentProfileRequest studentProfile)
        {
            if (studentProfile == null)
            {
                return BadRequest("Invalid student profile data.");
            }
            var group = await _groupService.GetGroupById(studentProfile.Group.Id);

            ApplicationUser userToLink;
            var existingUser = await _userManager.FindByNameAsync(studentProfile.UserName);
            if (existingUser == null)
            {
                // Якщо користувача не існує, створюємо нового користувача Identity
                var newUser = new ApplicationUser
                {
                    UserName = studentProfile.UserName,
                    Email = studentProfile.UserName + "@university.com" // Можливо, вам потрібно додати поле Email до TeacherProfileRequest
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
            var newStudentProfileId = Guid.NewGuid().ToString();

            var (profile, error) = StudentProfile.Create(
                newStudentProfileId,
                userToLink.Id,
                group,
                UserDetails.Create(userToLink.Id, studentProfile.UserName, studentProfile.FirstName, studentProfile.LastName, studentProfile.MiddleName ).userDatails
            );
            var studentProfileId = await _studentProfileService.CreateStudentProfile(profile);
            return Ok(studentProfileId);
        }

        // PUT: /StudentProfileUpdate/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateStudentProfile(string id, [FromBody] StudentProfileRequest studentProfile)
        {
            if (studentProfile == null || id == null)
            {
                return BadRequest("Invalid student profile data.");
            }
            var updatedProfileId = await _studentProfileService.UpdateStudentProfile(id, studentProfile.FirstName, studentProfile.LastName, studentProfile.MiddleName, studentProfile.Group.Id);
            return Ok(updatedProfileId);
        }

        // DELETE: /StudentProfile/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteStudentProfile(string id)
        {
            var deletedProfileId = await _studentProfileService.DeleteStudentProfile(id);
            if (deletedProfileId == null)
            {
                return NotFound();
            }
            return Ok(deletedProfileId);
        }
        // GET: /StudentProfile/Group/{groupId}
        [HttpGet("Group/{groupId}")]
        public async Task<ActionResult<IEnumerable<StudentProfileRequest>>> GetStudentProfilesByGroup(Guid groupId)
        {
            var studentProfiles = await _studentProfileService.GetStudentProfilesByGroup(groupId);
            if (studentProfiles == null || !studentProfiles.Any())
            {
                return NotFound();
            }
            var studentProfileResponses = studentProfiles.Select(sp => new StudentProfileRequest(
                sp.ApplicationUserId,
                sp.UserDetails.FirstName,
                sp.UserDetails.LastName,
                sp.UserDetails.Patronymic,
                sp.Group
            ));

            return Ok(studentProfileResponses);
        }

        // GET: /StudentProfile/UserDetails
        [HttpGet("UserDetails")]
        public async Task<ActionResult<StudentProfileResponse>> GetStudentProfileByUserDetails([FromBody] UserDetailsRequest userDetailsRequest)
        {
            if (userDetailsRequest == null)
            {
                return BadRequest("Invalid user details.");
            }
            var userDetails = UserDetails.Create(
                userDetailsRequest.Id,
                userDetailsRequest.UserName,
                userDetailsRequest.FirstName,
                userDetailsRequest.LastName,
                userDetailsRequest.MiddleName
            ).userDatails;

            var studentProfile = await _studentProfileService.GetStudentProfileByUserDetails(userDetails);
            if (studentProfile == null)
            {
                return NotFound();
            }
            var studentProfileResponse = new StudentProfileResponse(
                studentProfile.ApplicationUserId,
                studentProfile.UserDetails.FirstName,
                studentProfile.UserDetails.LastName,
                studentProfile.UserDetails.Patronymic,
                studentProfile.Group.Institute.Id,
                studentProfile.Group.Id
            );
            return Ok(studentProfileResponse);
        }
        // GET: /StudentProfile/Page/{pageNumber}/{pageSize}
        [HttpGet("Page/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<StudentProfileResponse>>> GetStudentProfiles(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and size must be greater than zero.");
            }
            var studentProfiles = await _studentProfileService.GetStudentProfiles(pageNumber, pageSize);
            if (studentProfiles == null || !studentProfiles.Any())
            {
                return NotFound();
            }
            var studentProfileResponses = studentProfiles.Select(sp => new StudentProfileResponse(
                sp.ApplicationUserId,
                sp.UserDetails.FirstName,
                sp.UserDetails.LastName,
                sp.UserDetails.Patronymic,
                sp.Group.Institute.Id,
                sp.Group.Id
            ));
            return Ok(studentProfileResponses);
        }

    }
}
