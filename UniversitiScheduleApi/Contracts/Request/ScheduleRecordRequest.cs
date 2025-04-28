using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record ScheduleRecordRequest
    (
        DateOnly Date,
        string AdditionalData,
        ClassTime ClassTime,
        Classroom Classroom 
    );
}
