using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.RepositoryInterfaces
{
    public interface ITeacherProfileRepository
    {
        Task<string> Create(TeacherProfile teacherProfile);
        Task<string> Delete(string id);
        Task<IEnumerable<TeacherProfile>> Get();
        Task<string> Update(string id, string firstName, string lastName, string patronymic, Guid instituteId);
    }
}