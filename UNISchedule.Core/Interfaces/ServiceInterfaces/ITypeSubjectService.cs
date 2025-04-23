using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface ITypeSubjectService
    {
        Task<Guid> CreateTypeSubject(TypeSubject typeSubject);
        Task<Guid> DeleteTypeSubject(Guid id);
        Task<IEnumerable<TypeSubject>> GetAllTypeSubjects();
        Task<TypeSubject> GetTypeSubjectById(Guid id);
        Task<TypeSubject> GetTypeSubjectByName(string type);
        Task<Guid> UpdateTypeSubject(Guid id, string name);
    }
}