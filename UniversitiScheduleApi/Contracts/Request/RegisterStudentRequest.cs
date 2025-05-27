
using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record RegisterStudentRequest(
        string FirstName,
        string LastName,
        string Patronymic,
        string Email,
        string Password,
        Group Group
    );


}
