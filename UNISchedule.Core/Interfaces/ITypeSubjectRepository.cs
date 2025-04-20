using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces
{
    public interface ITypeSubjectRepository
    {
        Task<Guid> Create(TypeSubject typeSubject);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<TypeSubject>> Get();
        Task<Guid> Update(Guid id, string type);
    }
}