using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Response
{
    public record UserDtoResponse(
        string Id,
        string FirstName,
        string LastName,
        string Patronymic,
        string Email
    );


}
