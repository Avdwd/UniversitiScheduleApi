using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class SubjectAssignmentRepository
    {
        private readonly UniScheduleDbContext _context;
        public SubjectAssignmentRepository(UniScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SubjectAssignmentEntity>> GetAllSubjectAssignmentsAsync()
        {
            return await _context.SubjectAssignmentEntities.ToListAsync();
        }
        public async Task<SubjectAssignmentEntity> GetSubjectAssignmentByIdAsync(Guid id)
        {
            return await _context.SubjectAssignmentEntities.FindAsync(id);
        }
        public async Task AddSubjectAssignmentAsync(SubjectAssignmentEntity subjectAssignment)
        {
            await _context.SubjectAssignmentEntities.AddAsync(subjectAssignment);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateSubjectAssignmentAsync(SubjectAssignmentEntity subjectAssignment)
        {
            _context.SubjectAssignmentEntities.Update(subjectAssignment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSubjectAssignmentAsync(Guid id)
        {
            var subjectAssignment = await GetSubjectAssignmentByIdAsync(id);
            if (subjectAssignment != null)
            {
                _context.SubjectAssignmentEntities.Remove(subjectAssignment);
                await _context.SaveChangesAsync();
            }
        }


        //pagination
        public async Task<IEnumerable<SubjectAssignmentEntity>> GetSubjectAssignmentsByPageAsync(int pageNumber, int pageSize)
        {
            return await _context.SubjectAssignmentEntities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
