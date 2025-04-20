using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly UniScheduleDbContext _context;
        public SubjectRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> Get()
        {
            var subjectEntities = await _context.SubjectEntities
                .AsNoTracking()
                .ToListAsync();

            var subjects = subjectEntities
                .Select(s => Subject.Create(s.Id, s.Name).subject)
                .ToList();


            return subjects;
        }
        public async Task<Guid> Create(Subject subject)
        {
            var subjectEntity = new SubjectEntity
            {
                Id = subject.Id,
                Name = subject.Name
            };
            await _context.SubjectEntities.AddAsync(subjectEntity);
            await _context.SaveChangesAsync();
            return subjectEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name)
        {
            await _context.SubjectEntities
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.Name, s => name));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.SubjectEntities
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
