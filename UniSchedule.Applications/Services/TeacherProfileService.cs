using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;

namespace UNISchedule.Applications.Services
{
    public class TeacherProfileService : ITeacherProfileService
    {
        // Implement methods for managing teacher profiles
        // For example, Create, Update, Delete, GetAll, GetById, etc.
        // These methods will interact with the TeacherProfileRepository to perform CRUD operations
        // and return the results to the caller.
        // You can also implement any additional business logic needed for teacher profiles here.

        private readonly ITeacherProfileRepository _teacherProfileRepository;
        public TeacherProfileService(ITeacherProfileRepository teacherProfileRepository)
        {
            _teacherProfileRepository = teacherProfileRepository;
        }
        //Method to create a teacher profile
        public async Task<string> CreateTeacherProfile(TeacherProfile teacherProfile)
        {
            return await _teacherProfileRepository.Create(teacherProfile);
        }
        //Method to delete a teacher profile
        public async Task<string> DeleteTeacherProfile(string id)
        {
            return await _teacherProfileRepository.Delete(id);
        }
        //Method to get all teacher profiles
        public async Task<IEnumerable<TeacherProfile>> GetAllTeacherProfiles()
        {
            return await _teacherProfileRepository.Get();
        }
        //Method to update a teacher profile
        public async Task<string> UpdateTeacherProfile(string id, string firstName, string lastName, string patronymic, Guid instituteId)
        {
            return await _teacherProfileRepository.Update(id, firstName, lastName, patronymic, instituteId);
        }
        //Method to get a teacher profile by id
        public async Task<TeacherProfile> GetTeacherProfileById(string id)
        {
            var teacherProfiles = await _teacherProfileRepository.Get();
            return teacherProfiles.FirstOrDefault(c => c.UserDetails.Id == id);
        }
        //Method to get a teacher profile by institute
        public async Task<IEnumerable<TeacherProfile>> GetTeacherProfilesByInstitute(Guid instituteId)
        {
            var teacherProfiles = await _teacherProfileRepository.Get();
            return teacherProfiles.Where(c => c.Institute.Id == instituteId);
        }
        //Method to get a teacher profile by user details
        public async Task<TeacherProfile> GetTeacherProfileByUserDetails(UserDetails userDetails)
        {
            var teacherProfiles = await _teacherProfileRepository.Get();
            return teacherProfiles.FirstOrDefault(c => c.UserDetails.Id == userDetails.Id);
        }
        //pagination
        public async Task<IEnumerable<TeacherProfile>> GetTeacherProfiles(int pageNumber, int pageSize)
        {
            var teacherProfiles = await _teacherProfileRepository.Get();
            return teacherProfiles.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
