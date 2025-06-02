
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request.RefRequests;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record RegisterStudentRequest(
        string FirstName,
        string LastName,
        string Patronymic,
        string Email,
        string Password,
        GroupRefRequest Group
    );


}
