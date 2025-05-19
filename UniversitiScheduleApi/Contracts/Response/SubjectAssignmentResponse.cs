using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Response
{
    public record SubjectAssignmentResponse
    (
        Guid Id,
        ScheduleRecord ScheduleRecord,
        Group Group,
        Subject Subject,
        //тут прописати викладача 
        TeacherProfile TeacherProfile,
        TypeSubject TypeSubject
    );
}
