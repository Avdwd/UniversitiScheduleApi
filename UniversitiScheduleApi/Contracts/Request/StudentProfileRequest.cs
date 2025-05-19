namespace UniversitiScheduleApi.Contracts.Request
{
    public record StudentProfileRequest
    (
        string FirstName,
        string LastName,
        string MiddleName,
        string PhoneNumber,
        string Email,
        string InstituteId,
        string GroupId
    );
}
