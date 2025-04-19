using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class SubjectRepository
    {
        private readonly UniScheduleDbContext _context;
        public SubjectRepository(UniScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SubjectEntity>> GetAllSubjectsAsync()
        {
            return await _context.SubjectEntities.ToListAsync();
        }
        public async Task<SubjectEntity> GetSubjectByIdAsync(Guid id)
        {
            return await _context.SubjectEntities.FindAsync(id);
        }
        public async Task AddSubjectAsync(SubjectEntity subject)
        {
            await _context.SubjectEntities.AddAsync(subject);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateSubjectAsync(SubjectEntity subject)
        {
            _context.SubjectEntities.Update(subject);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSubjectAsync(Guid id)
        {
            var subject = await GetSubjectByIdAsync(id);
            if (subject != null)
            {
                _context.SubjectEntities.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }


        //pagination
        public async Task<IEnumerable<SubjectEntity>> GetSubjectsByPageAsync(int pageNumber, int pageSize)
        {
            return await _context.SubjectEntities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
