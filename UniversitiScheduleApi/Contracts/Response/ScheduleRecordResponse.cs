using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Response
{
    public record ScheduleRecordResponse(
        Guid Id,
        DateOnly Date,
        string AdditionalData,
        ClassTime ClassTime,
        Classroom Classroom
    );

}
