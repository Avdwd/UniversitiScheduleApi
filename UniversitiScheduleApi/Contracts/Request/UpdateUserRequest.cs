namespace UniversitiScheduleApi.Contracts.Request
{
    public record UpdateUserRequest(
        string UserId,
        string FirstName,
        string LastName,
        string Patronymic
    );

}
