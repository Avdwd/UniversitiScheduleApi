using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IScheduleRecordService
    {
        Task<Guid> CreateScheduleRecord(ScheduleRecord scheduleRecord);
        Task<Guid> DeleteScheduleRecord(Guid id);
        Task<IEnumerable<ScheduleRecord>> GetAllScheduleRecords();
        Task<ScheduleRecord> GetScheduleRecordByClassroom(Classroom classroom);
        Task<ScheduleRecord> GetScheduleRecordByClassTime(ClassTime classTime);
        Task<ScheduleRecord> GetScheduleRecordByDate(DateOnly date);
        Task<ScheduleRecord> GetScheduleRecordById(Guid id);
        Task<IEnumerable<ScheduleRecord>> GetScheduleRecords(int pageNumber, int pageSize);
        Task<Guid> UpdateScheduleRecord(Guid id, DateOnly date, string addionalData, Guid classTimeId, Guid classroomId);
    }
}