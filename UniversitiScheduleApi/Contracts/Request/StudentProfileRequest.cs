using UNISchedule.Core.Models;

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
