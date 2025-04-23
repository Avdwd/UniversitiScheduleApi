using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;

namespace UNISchedule.Applications.Services
{
    public class ScheduleRecordService : IScheduleRecordService
    {
        private readonly IScheduleRecordRepository _scheduleRecordRepository;
        public ScheduleRecordService(IScheduleRecordRepository scheduleRecordRepository)
        {
            _scheduleRecordRepository = scheduleRecordRepository;
        }
        //Method to create a schedule record
        public async Task<Guid> CreateScheduleRecord(ScheduleRecord scheduleRecord)
        {
            return await _scheduleRecordRepository.Create(scheduleRecord);
        }
        //Method to delete a schedule record
        public async Task<Guid> DeleteScheduleRecord(Guid id)
        {
            return await _scheduleRecordRepository.Delete(id);
        }
        //Method to get all schedule records
        public async Task<IEnumerable<ScheduleRecord>> GetAllScheduleRecords()
        {
            return await _scheduleRecordRepository.Get();
        }
        //Method to update a schedule record
        public async Task<Guid> UpdateScheduleRecord(Guid id, DateOnly date, string addionalData, Guid classTimeId, Guid classroomId)
        {
            return await _scheduleRecordRepository.Update(id, date, addionalData, classTimeId, classroomId);
        }
        //Method to get a schedule record by id
        public async Task<ScheduleRecord> GetScheduleRecordById(Guid id)
        {
            var scheduleRecords = await _scheduleRecordRepository.Get();
            return scheduleRecords.FirstOrDefault(c => c.Id == id);
        }
        // Method to get a schedule record by date
        public async Task<ScheduleRecord> GetScheduleRecordByDate(DateOnly date)
        {
            var scheduleRecords = await _scheduleRecordRepository.Get();
            return scheduleRecords.FirstOrDefault(c => c.Date == date);
        }
        // Method to get a schedule record by class time
        public async Task<ScheduleRecord> GetScheduleRecordByClassTime(ClassTime classTime)
        {
            var scheduleRecords = await _scheduleRecordRepository.Get();
            return scheduleRecords.FirstOrDefault(c => c.ClassTime == classTime);
        }
        // Method to get a schedule record by classroom
        public async Task<ScheduleRecord> GetScheduleRecordByClassroom(Classroom classroom)
        {
            var scheduleRecords = await _scheduleRecordRepository.Get();
            return scheduleRecords.FirstOrDefault(c => c.Classroom == classroom);
        }
        // Method pagination
        public async Task<IEnumerable<ScheduleRecord>> GetScheduleRecords(int pageNumber, int pageSize)
        {
            var scheduleRecords = await _scheduleRecordRepository.Get();
            return scheduleRecords.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
