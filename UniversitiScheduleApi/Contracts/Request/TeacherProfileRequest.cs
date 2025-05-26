using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record TeacherProfileRequest(
        string FirstName,
        string LastName,
        string MiddleName,
        string UserName,
        Institute Institute
    );
}
