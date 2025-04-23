using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public class ClassTimeService : IClassTimeService
    {
        private readonly IClassTimeRepository _classTimeRepository;
        public ClassTimeService(IClassTimeRepository classTimeRepository)
        {
            _classTimeRepository = classTimeRepository;
        }
        //Method to create a class time
        public async Task<Guid> CreateClassTime(ClassTime classTime)
        {
            return await _classTimeRepository.Create(classTime);
        }
        //Method to delete a class time
        public async Task<Guid> DeleteClassTime(Guid id)
        {
            return await _classTimeRepository.Delete(id);
        }
        //Method to get all class times
        public async Task<IEnumerable<ClassTime>> GetAllClassTimes()
        {
            return await _classTimeRepository.Get();
        }
        //Method to update a class time
        public async Task<Guid> UpdateClassTime(Guid id, string timeFrame)
        {
            return await _classTimeRepository.Update(id, timeFrame);
        }
        //Method to get a class time by id
        public async Task<ClassTime> GetClassTimeById(Guid id)
        {
            var classTimes = await _classTimeRepository.Get();
            return classTimes.FirstOrDefault(c => c.Id == id);

        }
        // Method to get a class time by time frame
        public async Task<ClassTime> GetClassTimeByTimeFrame(string timeFrame)
        {
            var classTimes = await _classTimeRepository.Get();
            return classTimes.FirstOrDefault(c => c.Timeframe == timeFrame);
        }
    }
}
