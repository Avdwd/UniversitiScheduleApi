using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request.RefRequests;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record TeacherProfileRequest(
        string FirstName,
        string LastName,
        string MiddleName,
        string UserName,
        InstituteRefRequest Institute // guid
    );
}
