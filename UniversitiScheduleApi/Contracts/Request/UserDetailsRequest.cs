namespace UniversitiScheduleApi.Contracts.Request
{
    public record UserDetailsRequest
    (
        string Id,
        string UserName,
        string FirstName,
        string LastName,
        string MiddleName
    );

}
