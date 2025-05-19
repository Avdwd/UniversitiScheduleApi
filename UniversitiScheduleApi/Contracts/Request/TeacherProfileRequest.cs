namespace UniversitiScheduleApi.Contracts.Request
{
    public record TeacherProfileRequest(
        string FirstName,
        string LastName,
        string MiddleName,
        string PhoneNumber,
        string Email,
        string InstituteId
    );
}
