using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface ITeacherProfileService
    {
        Task<string> CreateTeacherProfile(TeacherProfile teacherProfile);
        Task<string> DeleteTeacherProfile(string id);
        Task<IEnumerable<TeacherProfile>> GetAllTeacherProfiles();
        Task<TeacherProfile> GetTeacherProfileById(string id);
        Task<TeacherProfile> GetTeacherProfileByUserDetails(UserDetails userDetails);
        Task<IEnumerable<TeacherProfile>> GetTeacherProfilesByInstitute(Guid instituteId);
        Task<string> UpdateTeacherProfile(string id, string firstName, string lastName, string patronymic, Guid instituteId);
        Task<IEnumerable<TeacherProfile>> GetTeacherProfiles(int pageNumber, int pageSize);
    }
}