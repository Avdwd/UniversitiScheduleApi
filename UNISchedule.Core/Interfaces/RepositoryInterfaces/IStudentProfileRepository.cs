using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.RepositoryInterfaces
{
    public interface IStudentProfileRepository
    {
        Task<string> Create(StudentProfile studentProfile);
        Task<string> Delete(string id);
        Task<IEnumerable<StudentProfile>> Get();
        Task<string> Update(string id, string firstName, string lastName, string patronymic, Guid groupId);
    }
}