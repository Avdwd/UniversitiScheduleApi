using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IInstituteService
    {
        Task<Guid> CreateInstitute(Institute institute);
        Task<Guid> DeleteInstitute(Guid id);
        Task<IEnumerable<Institute>> GetAllInstitutes();
        Task<Institute> GetInstituteById(Guid id);
        Task<Institute> GetInstituteByName(string name);
        Task<Guid> UpdateInstitute(Guid id, string name);
    }
}



