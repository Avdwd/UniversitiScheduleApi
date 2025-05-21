namespace UniversitiScheduleApi.Contracts.Request
{
    public record LoginRequest(
        string Email,
        string Password,
        bool RememberMe = false
    );

}
