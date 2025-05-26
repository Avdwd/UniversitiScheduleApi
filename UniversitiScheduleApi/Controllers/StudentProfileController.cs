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
    public class StudentProfileController : ControllerBase
    {
        public readonly IStudentProfileService _studentProfileService;
        public StudentProfileController(IStudentProfileService studentProfileService)
        {
            _studentProfileService = studentProfileService;
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
            var applicationUserId = new ApplicationUser();
            var (profile, error) = StudentProfile.Create(

                applicationUserId.Id,
                studentProfile.Group,
                UserDetails.Create(applicationUserId.Id, studentProfile.UserName, studentProfile.FirstName, studentProfile.LastName, studentProfile.MiddleName ).userDatails
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
