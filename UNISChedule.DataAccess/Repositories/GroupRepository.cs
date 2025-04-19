using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class GroupRepository
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
                .ToListAsync();
            // Map GroupEntity to Group
            var groups = groupEntities
                .Select(g => Group.Create(g.Id, g.Name, g.InstituteId).group)
                .ToList();

            return groups;
        }
    }
}
