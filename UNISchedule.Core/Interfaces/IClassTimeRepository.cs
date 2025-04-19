using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces
{
    public interface IClassTimeRepository
    {
        Task<Guid> Create(ClassTime classTime);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<ClassTime>> Get();
        Task<Guid> Update(Guid id, string timeFrame);
    }
}