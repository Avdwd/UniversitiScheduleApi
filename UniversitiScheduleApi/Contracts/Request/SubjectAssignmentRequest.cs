using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record SubjectAssignmentRequest(
        ScheduleRecord ScheduleRecord,
        Group Group,
        Subject Subject,
        //тут прописати викладача 
        TeacherProfile TeacherProfile,
        TypeSubject TypeSubject
    );

}
