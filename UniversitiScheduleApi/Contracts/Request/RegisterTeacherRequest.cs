using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request.RefRequests;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record RegisterTeacherRequest(
        string FirstName,
        string LastName,
        string Patronymic,
        string Email,
        string Password,
        string TeacherProfileId,
        InstituteRefRequest Institute
    );
    
}
