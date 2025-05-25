using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;

namespace UNISchedule.Applications.Services
{
    public class StudentProfileService : IStudentProfileService
    {
        
        private readonly IStudentProfileRepository _studentProfileRepository;
        public StudentProfileService(IStudentProfileRepository studentProfileRepository)
        {
            _studentProfileRepository = studentProfileRepository;
        }
        //Method to create a student profile
        public async Task<string> CreateStudentProfile(StudentProfile studentProfile)
        {
            return await _studentProfileRepository.Create(studentProfile);
        }
        //Method to delete a student profile
        public async Task<string> DeleteStudentProfile(string id)
        {
            return await _studentProfileRepository.Delete(id);
        }
        //Method to get all student profiles
        public async Task<IEnumerable<StudentProfile>> GetAllStudentProfiles()
        {
            return await _studentProfileRepository.Get();
        }

        //Method to update a student profile
        public async Task<string> UpdateStudentProfile(string id, string firstName, string lastName, string patronymic, Guid groupId)
        {
            return await _studentProfileRepository.Update(id, firstName, lastName, patronymic, groupId);
        }
        //Method to get a student profile by id
        public async Task<StudentProfile> GetStudentProfileById(string id)
        {
            var studentProfiles = await _studentProfileRepository.Get();
            return studentProfiles.FirstOrDefault(c => c.UserDetails.Id == id);
        }
        //Method to get a student profile by group
        public async Task<IEnumerable<StudentProfile>> GetStudentProfilesByGroup(Guid groupId)
        {
            var studentProfiles = await _studentProfileRepository.Get();
            return studentProfiles.Where(c => c.Group.Id == groupId);
        }
        //Method to get a student profile by user details
        public async Task<StudentProfile> GetStudentProfileByUserDetails(UserDetails userDetails)
        {
            var studentProfiles = await _studentProfileRepository.Get();
            return studentProfiles.FirstOrDefault(c => c.UserDetails.Id == userDetails.Id);
        }
        //pagination
        public async Task<IEnumerable<StudentProfile>> GetStudentProfiles(int pageNumber, int pageSize)
        {
            var studentProfiles = await _studentProfileRepository.Get();
            return studentProfiles.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

    }
}
