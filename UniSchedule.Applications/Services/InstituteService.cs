using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;

namespace UNISchedule.Applications.Services
{
    public class InstituteService : IInstituteService
    {
        private readonly IInstituteRepository _instituteRepository;
        public InstituteService(IInstituteRepository instituteRepository)
        {
            _instituteRepository = instituteRepository;
        }
        //Method to create a institute
        public async Task<Guid> CreateInstitute(Institute institute)
        {
            return await _instituteRepository.Create(institute);
        }
        //Method to delete a institute
        public async Task<Guid> DeleteInstitute(Guid id)
        {
            return await _instituteRepository.Delete(id);
        }
        //Method to get all institutes
        public async Task<IEnumerable<Institute>> GetAllInstitutes()
        {
            return await _instituteRepository.Get();
        }
        //Method to update a institute
        public async Task<Guid> UpdateInstitute(Guid id, string name)
        {
            return await _instituteRepository.Update(id, name);
        }
        //Method to get a institute by id
        public async Task<Institute> GetInstituteById(Guid id)
        {
            var institutes = await _instituteRepository.Get();
            return institutes.FirstOrDefault(c => c.Id == id);
        }
        // Method to get a institute by name
        public async Task<Institute> GetInstituteByName(string name)
        {
            var institutes = await _instituteRepository.Get();
            return institutes.FirstOrDefault(c => c.Name == name);
        }

    }
}
