using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Entities;
using UNISchedule.Core.Models;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;

namespace UNISchedule.DataAccess.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly UniScheduleDbContext _context;
        public ClassroomRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Classroom>> Get()
        {
            var classroomEntities = await _context.ClassroomEntities
                .AsNoTracking()
                .ToListAsync();
            // Map ClassroomEntity to Classroom
            var classrooms = classroomEntities
                .Select(c => Classroom.Create(c.Id, c.Number, c.Building).classroom)
                .ToList();

            return classrooms;
        }

        public async Task<Guid> Create(Classroom classroom)
        {
            var classroomEntity = new ClassroomEntity
            {
                Id = classroom.Id,
                Number = classroom.Number,
                Building = classroom.Building
            };
            await _context.ClassroomEntities.AddAsync(classroomEntity);
            await _context.SaveChangesAsync();
            // Map Classroom to ClassroomEntity
            return classroomEntity.Id;
        }



        public async Task<Guid> Update(Guid id, int number, int building)
        {
            await _context.ClassroomEntities
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(c => c.Number, c => number)
                    .SetProperty(c => c.Building, c => building));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.ClassroomEntities
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }




    }
}
