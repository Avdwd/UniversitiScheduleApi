using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces
{
    public interface ISubjectAssignmentRepository
    {
        Task<Guid> Create(SubjectAssignment subjectAssignment);
        Task Delete(Guid id);
        Task<IEnumerable<SubjectAssignment>> Get();
        Task<Guid> Update(Guid id, Guid scheduleRecordId, Guid groupId, Guid subjectId, Guid typeSubjectId);
    }
}