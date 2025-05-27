using System.Text.RegularExpressions;

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
