using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.RepositoryInterfaces
{
    public interface ISubjectAssignmentRepository
    {
        Task<Guid> Create(SubjectAssignment subjectAssignment);
        Task<Guid> Delete(Guid id);
        Task<IEnumerable<SubjectAssignment>> Get();
        Task<Guid> Update(Guid id, Guid scheduleRecordId, Guid groupId, Guid subjectId, Guid typeSubjectId);
    }
}