using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class TypeSubjectRepository
    {
        private readonly UniScheduleDbContext _context;
        public TypeSubjectRepository(UniScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TypeSubjectEntity>> GetAllTypeSubjectsAsync()
        {
            return await _context.TypeSubjectEntities.ToListAsync();
        }
        public async Task<TypeSubjectEntity> GetTypeSubjectByIdAsync(Guid id)
        {
            return await _context.TypeSubjectEntities.FindAsync(id);
        }
        public async Task AddTypeSubjectAsync(TypeSubjectEntity typeSubject)
        {
            await _context.TypeSubjectEntities.AddAsync(typeSubject);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateTypeSubjectAsync(TypeSubjectEntity typeSubject)
        {
            _context.TypeSubjectEntities.Update(typeSubject);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTypeSubjectAsync(Guid id)
        {
            var typeSubject = await GetTypeSubjectByIdAsync(id);
            if (typeSubject != null)
            {
                _context.TypeSubjectEntities.Remove(typeSubject);
                await _context.SaveChangesAsync();
            }
        }
    }
}
