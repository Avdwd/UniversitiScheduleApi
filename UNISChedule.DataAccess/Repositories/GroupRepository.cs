using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly UniScheduleDbContext _context;
        public GroupRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> Get()
        {
            var groupEntities = await _context.GroupEntities
                .AsNoTracking()
                .Include(g => g.InstituteEntity)
                .ToListAsync();

            var groups = groupEntities.Select(g =>
            {
                var institute = Institute.Create(g.InstituteEntity.Id, g.InstituteEntity.Name).institute;
                return Group.Create(g.Id, g.Name, institute).group;
            });

            return groups;
        }

        public async Task<Guid> Create(Group group)
        {
            var groupEntity = new GroupEntity
            {
                Id = group.Id,
                Name = group.Name,
                InstituteEntityId = group.Institute.Id
            };

            await _context.GroupEntities.AddAsync(groupEntity);
            await _context.SaveChangesAsync();
            return groupEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, Guid instituteId)
        {
            await _context.GroupEntities
                .Where(g => g.Id == id)
                .ExecuteUpdateAsync(g => g
                    .SetProperty(g => g.Name, g => name)
                    .SetProperty(g => g.InstituteEntityId, g => instituteId));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.GroupEntities
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
