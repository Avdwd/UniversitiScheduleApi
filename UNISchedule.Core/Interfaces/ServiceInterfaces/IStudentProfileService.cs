using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IStudentProfileService
    {
        Task<string> CreateStudentProfile(StudentProfile studentProfile);
        Task<string> DeleteStudentProfile(string id);
        Task<IEnumerable<StudentProfile>> GetAllStudentProfiles();
        Task<StudentProfile> GetStudentProfileById(string id);
        Task<StudentProfile> GetStudentProfileByUserDetails(UserDetails userDetails);
        Task<IEnumerable<StudentProfile>> GetStudentProfilesByGroup(Guid groupId);
        Task<string> UpdateStudentProfile(string id, string firstName, string lastName, string patronymic, Guid groupId);
        Task<IEnumerable<StudentProfile>> GetStudentProfiles(int pageNumber, int pageSize);
    }
}