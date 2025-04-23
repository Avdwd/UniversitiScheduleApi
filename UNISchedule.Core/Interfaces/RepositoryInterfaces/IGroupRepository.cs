using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.RepositoryInterfaces
{
    public interface IGroupRepository
    {
        Task<Guid> Create(Group group);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<Group>> Get();
        Task<Guid> Update(Guid id, string name, Guid instituteId);
    }
}