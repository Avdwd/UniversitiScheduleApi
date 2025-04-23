using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IClassTimeService
    {
        Task<Guid> CreateClassTime(ClassTime classTime);
        Task<Guid> DeleteClassTime(Guid id);
        Task<IEnumerable<ClassTime>> GetAllClassTimes();
        Task<ClassTime> GetClassTimeById(Guid id);
        Task<ClassTime> GetClassTimeByTimeFrame(string timeFrame);
        Task<Guid> UpdateClassTime(Guid id, string timeFrame);
    }
}