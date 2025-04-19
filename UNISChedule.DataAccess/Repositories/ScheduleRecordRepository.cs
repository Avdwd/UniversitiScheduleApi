using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class ScheduleRecordRepository
    {
        private readonly UniScheduleDbContext _context;
        public ScheduleRecordRepository(UniScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ScheduleRecordEntity>> GetAllScheduleRecordsAsync()
        {
            return await _context.ScheduleRecordEntities.ToListAsync();
        }
        public async Task<ScheduleRecordEntity> GetScheduleRecordByIdAsync(Guid id)
        {
            return await _context.ScheduleRecordEntities.FindAsync(id);
        }
        public async Task AddScheduleRecordAsync(ScheduleRecordEntity scheduleRecord)
        {
            await _context.ScheduleRecordEntities.AddAsync(scheduleRecord);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateScheduleRecordAsync(ScheduleRecordEntity scheduleRecord)
        {
            _context.ScheduleRecordEntities.Update(scheduleRecord);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteScheduleRecordAsync(Guid id)
        {
            var scheduleRecord = await GetScheduleRecordByIdAsync(id);
            if (scheduleRecord != null)
            {
                _context.ScheduleRecordEntities.Remove(scheduleRecord);
                await _context.SaveChangesAsync();
            }
        }

        //pagination
        public async Task<IEnumerable<ScheduleRecordEntity>> GetScheduleRecordsByPageAsync(int pageNumber, int pageSize)
        {
            return await _context.ScheduleRecordEntities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
