using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.RepositoryInterfaces
{
    public interface ISubjectRepository
    {
        Task<Guid> Create(Subject subject);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<Subject>> Get();
        Task<Guid> Update(Guid id, string name);
    }
}