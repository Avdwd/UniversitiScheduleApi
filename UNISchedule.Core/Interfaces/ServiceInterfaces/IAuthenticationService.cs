using UNISchedule.Core.Models;

namespace UNISchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResultDto> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<RegisterResultDto> RegisterStudentAsync(string email, string password, string lastName, string firstName, string patronymic, Group group);
        Task<RegisterResultDto> RegisterTeacherAsync(string email, string password, string lastName, string firstName, string patronymic, Institute institute);
    }
}