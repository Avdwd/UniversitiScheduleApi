using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class TypeSubjectRepository : ITypeSubjectRepository
    {
        private readonly UniScheduleDbContext _context;
        public TypeSubjectRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TypeSubject>> Get()
        {
            var typeSubjectEntities = await _context.TypeSubjectEntities
                .AsNoTracking()
                .ToListAsync();

            var typeSubjects = typeSubjectEntities
                .Select(ts => TypeSubject.Create(ts.Id, ts.Type).typeSubject)
                .ToList();
            return typeSubjects;
        }

        public async Task<Guid> Create(TypeSubject typeSubject)
        {
            var typeSubjectEntity = new TypeSubjectEntity
            {
                Id = typeSubject.Id,
                Type = typeSubject.Type
            };
            await _context.TypeSubjectEntities.AddAsync(typeSubjectEntity);
            await _context.SaveChangesAsync();
            return typeSubjectEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string type)
        {
            await _context.TypeSubjectEntities
                .Where(ts => ts.Id == id)
                .ExecuteUpdateAsync(ts => ts
                    .SetProperty(ts => ts.Type, ts => type));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.TypeSubjectEntities
                .Where(ts => ts.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
