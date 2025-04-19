using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces
{
    public interface IClassroomRepository
    {
        Task<Guid> Create(Classroom classroom);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<Classroom>> Get();
        Task<Guid> Update(Guid id, int number, int building);
    }
}