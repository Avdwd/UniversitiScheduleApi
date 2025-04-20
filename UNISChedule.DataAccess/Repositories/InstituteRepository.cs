using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class InstituteRepository : IInstituteRepository
    {
        private readonly UniScheduleDbContext _context;
        public InstituteRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Institute>> Get()
        {
            var instituteEntities = await _context.InstituteEntities
                .AsNoTracking()
                .ToListAsync();
            var institutes = instituteEntities
                .Select(i => Institute.Create(i.Id, i.Name).institute)
                .ToList();
            return institutes;
        }

        public async Task<Guid> Create(Institute institute)
        {
            var instituteEntity = new InstituteEntity
            {
                Id = institute.Id,
                Name = institute.Name
            };
            await _context.InstituteEntities.AddAsync(instituteEntity);
            await _context.SaveChangesAsync();
            return instituteEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name)
        {
            await _context.InstituteEntities
                .Where(i => i.Id == id)
                .ExecuteUpdateAsync(i => i
                    .SetProperty(i => i.Name, i => name));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.InstituteEntities
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
