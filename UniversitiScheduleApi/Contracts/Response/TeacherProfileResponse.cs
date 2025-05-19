namespace UniversitiScheduleApi.Contracts.Response
{
    public record TeacherProfileResponse
    (
        string Id,
        string FirstName,
        string LastName,
        string MiddleName,
        string PhoneNumber,
        string Email,
        string InstituteId
    );
    
}
