using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request.RefRequests;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record ScheduleRecordRequest
    (
        DateOnly Date,
        string AdditionalData,
        ClassTimeRefRequest ClassTime,
        ClassroomRefRequest Classroom 
    );
}
