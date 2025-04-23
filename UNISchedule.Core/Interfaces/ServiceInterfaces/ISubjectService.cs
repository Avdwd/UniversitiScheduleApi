using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface ISubjectService
    {
        Task<Guid> CreateSubject(Subject subject);
        Task<Guid> DeleteSubject(Guid id);
        Task<IEnumerable<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectById(Guid id);
        Task<Subject> GetSubjectByName(string name);
        Task<IEnumerable<Subject>> GetSubjects(int pageNumber, int pageSize);
        Task<Guid> UpdateSubject(Guid id, string name);
    }
}