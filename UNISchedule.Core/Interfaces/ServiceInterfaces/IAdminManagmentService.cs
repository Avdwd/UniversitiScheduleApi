
namespace UNISchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IAdminManagmentService
    {
        Task<bool> AddAdminAsync(string email, string password);
        Task<bool> RemoveAdminAsync(string email);
    }
}