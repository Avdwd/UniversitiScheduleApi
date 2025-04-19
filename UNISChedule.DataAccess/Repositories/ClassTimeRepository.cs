using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;


namespace UNISchedule.DataAccess.Repositories
{
    public class ClassTimeRepository : IClassTimeRepository
    {
        private readonly UniScheduleDbContext _context;
        public ClassTimeRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassTime>> Get()
        {
            var classTimeEntity = await _context.ClassTimeEntities
                .AsNoTracking()
                .ToListAsync();
            // Map ClassTimeEntity to ClassTime
            var classTime = classTimeEntity
                .Select(c => ClassTime.Create(c.Id, c.Timeframe).classTime)
                .ToList();

            return classTime;
        }

        public async Task<Guid> Create(ClassTime classTime)
        {
            var classTimeEntity = new ClassTimeEntity
            {
                Id = classTime.Id,
                Timeframe = classTime.Timeframe
            };

            await _context.ClassTimeEntities.AddAsync(classTimeEntity);
            await _context.SaveChangesAsync();
            // Map ClassTime to ClassTimeEntity
            return classTimeEntity.Id;
        }



        public async Task<Guid> Update(Guid id, string timeFrame)
        {
            await _context.ClassTimeEntities
                .Where(ct => ct.Id == id)
                .ExecuteUpdateAsync(ct => ct
                    .SetProperty(ct => ct.Timeframe, c => timeFrame));

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.ClassTimeEntities
                .Where(ct => ct.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
