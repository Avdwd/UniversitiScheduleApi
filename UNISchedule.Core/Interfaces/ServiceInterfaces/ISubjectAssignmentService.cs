using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface ISubjectAssignmentService
    {
        Task<Guid> CreateSubjectAssignment(SubjectAssignment subjectAssignment);
        Task<Guid> DeleteSubjectAssignment(Guid id);
        Task<IEnumerable<SubjectAssignment>> GetAllSubjectAssignments();
        Task<SubjectAssignment> GetSubjectAssignmentByGroup(Group group);
        Task<SubjectAssignment> GetSubjectAssignmentByGroupAndScheduleRecord(Group group, ScheduleRecord scheduleRecord);
        Task<SubjectAssignment> GetSubjectAssignmentByGroupAndSubject(Group group, Subject subject);
        Task<SubjectAssignment> GetSubjectAssignmentByGroupAndTypeSubject(Group group, TypeSubject typeSubject);
        Task<SubjectAssignment> GetSubjectAssignmentById(Guid id);
        Task<SubjectAssignment> GetSubjectAssignmentByScheduleRecord(ScheduleRecord scheduleRecord);
        Task<SubjectAssignment> GetSubjectAssignmentBySubject(Subject subject);
        Task<SubjectAssignment> GetSubjectAssignmentByTypeSubject(TypeSubject typeSubject);
        Task<IEnumerable<SubjectAssignment>> GetSubjectAssignments(int pageNumber, int pageSize);
        Task<Guid> UpdateSubjectAssignment(Guid id, Guid scheduleRecordId, Guid groupId, Guid subjectId, Guid typeSubjectId);
    }
}