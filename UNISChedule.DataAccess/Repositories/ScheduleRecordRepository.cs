using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class ScheduleRecordRepository : IScheduleRecordRepository
    {
        private readonly UniScheduleDbContext _context;
        public ScheduleRecordRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduleRecord>> Get()
        {
            var scheduleRecordEntities = await _context.ScheduleRecordEntities
                .AsNoTracking()
                .Include(sr => sr.ClassroomEntity)
                .Include(sr => sr.ClassTime)
                .ToListAsync();

            // Map ScheduleRecordEntity to ScheduleRecord
            var scheduleRecords = scheduleRecordEntities
                .Select(sr =>
                {
                    var classroom = Classroom.Create(sr.ClassroomEntity.Id, sr.ClassroomEntity.Number, sr.ClassroomEntity.Building).classroom;
                    var classTime = ClassTime.Create(sr.ClassTime.Id, sr.ClassTime.Timeframe).classTime;
                    return ScheduleRecord.Create(sr.Id, sr.Date, sr.AdditionalData, classTime, classroom).scheduleRecord;
                });

            return scheduleRecords;
        }

        public async Task<Guid> Create(ScheduleRecord scheduleRecord)
        {
            var scheduleRecordEntity = new ScheduleRecordEntity
            {
                Id = scheduleRecord.Id,
                Date = scheduleRecord.Date,
                AdditionalData = scheduleRecord.AdditionalData,
                ClassTimeEntityId = scheduleRecord.ClassTime.Id,
                ClassroomEntityId = scheduleRecord.Classroom.Id
            };
            await _context.ScheduleRecordEntities.AddAsync(scheduleRecordEntity);
            await _context.SaveChangesAsync();
            return scheduleRecordEntity.Id;
        }

        public async Task<Guid> Update(Guid id, DateOnly date, string additionalData, Guid classTimeId, Guid classroomId)
        {
            await _context.ScheduleRecordEntities
                .Where(sr => sr.Id == id)
                .ExecuteUpdateAsync(sr => sr
                    .SetProperty(sr => sr.Date, sr => date)
                    .SetProperty(sr => sr.AdditionalData, sr => additionalData)
                    .SetProperty(sr => sr.ClassTimeEntityId, sr => classTimeId)
                    .SetProperty(sr => sr.ClassroomEntityId, sr => classroomId));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.ScheduleRecordEntities
                .Where(sr => sr.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
