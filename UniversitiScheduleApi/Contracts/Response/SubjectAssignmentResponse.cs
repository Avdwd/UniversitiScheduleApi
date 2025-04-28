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
        TypeSubject TypeSubject
    );
}
