using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request.RefRequests;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record StudentProfileRequest
    (
        string FirstName,
        string LastName,
        string MiddleName,
        string UserName,
        Group Group
    );
}
