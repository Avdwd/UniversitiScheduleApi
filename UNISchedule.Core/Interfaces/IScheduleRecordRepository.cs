using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces
{
    public interface IScheduleRecordRepository
    {
        Task<Guid> Create(ScheduleRecord scheduleRecord);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<ScheduleRecord>> Get();
        Task<Guid> Update(Guid id, DateOnly date, string additionalData, Guid classTimeId, Guid classroomId);
    }
}