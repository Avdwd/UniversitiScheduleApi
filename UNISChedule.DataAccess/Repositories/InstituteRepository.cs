using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class InstituteRepository
    {
        private readonly UniScheduleDbContext _context;
        public InstituteRepository(UniScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InstituteEntity>> GetAllInstitutesAsync()
        {
            return await _context.InstituteEntities.ToListAsync();
        }
        public async Task<InstituteEntity> GetInstituteByIdAsync(Guid id)
        {
            return await _context.InstituteEntities.FindAsync(id);
        }
        public async Task AddInstituteAsync(InstituteEntity institute)
        {
            await _context.InstituteEntities.AddAsync(institute);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateInstituteAsync(InstituteEntity institute)
        {
            _context.InstituteEntities.Update(institute);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteInstituteAsync(Guid id)
        {
            var institute = await GetInstituteByIdAsync(id);
            if (institute != null)
            {
                _context.InstituteEntities.Remove(institute);
                await _context.SaveChangesAsync();
            }
        }
    }
}
