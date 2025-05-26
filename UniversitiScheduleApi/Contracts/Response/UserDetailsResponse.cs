namespace UniversitiScheduleApi.Contracts.Response
{
    public record UserDetailsResponse
    (
        string Id,
        string UserName,
        string FirstName,
        string LastName,
        string Patronymic
    );
}
