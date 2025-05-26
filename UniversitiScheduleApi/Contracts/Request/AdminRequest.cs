namespace UniversitiScheduleApi.Contracts.Request
{
    public record AdminRequest(
        string FirstName,
        string LastName,
        string Patronymic,
        string UserName
    );

}
