using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request.RefRequests;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record SubjectAssignmentRequest(
        ScheduleRecordRefRequest ScheduleRecord,
        GroupRefRequest Group,
        SubjectRefRequest Subject,
        TeacherProfile TeacherProfile,
        TypeSubjectRefRequest TypeSubject
    );

}
