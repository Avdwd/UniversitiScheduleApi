namespace UniversitiScheduleApi.Contracts.Request
{
    public record CreateUserRequest
        (
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Patronymic,
        string RoleName = "User" // Default role is User, can be overridden
        );
    
}
