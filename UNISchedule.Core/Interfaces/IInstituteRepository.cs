using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces
{
    public interface IInstituteRepository
    {
        Task<Guid> Create(Institute institute);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<Institute>> Get();
        Task<Guid> Update(Guid id, string name);
    }
}