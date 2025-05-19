namespace UniversitiScheduleApi.Contracts.Response
{
    public record StudentProfileResponse(
        string Id,
        string FirstName,
        string LastName,
        string MiddleName,
        string PhoneNumber,
        string Email,
        string InstituteId,
        string GroupId
    );
    
}
