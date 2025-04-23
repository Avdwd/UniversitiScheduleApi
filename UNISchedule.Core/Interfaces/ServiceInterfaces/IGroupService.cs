using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IGroupService
    {
        Task<Guid> CreateGroup(Group group);
        Task<Guid> DeleteGroup(Guid id);
        Task<IEnumerable<Group>> GetAllGroups();
        Task<IEnumerable<Group>> GetClassrooms(int pageNumber, int pageSize);
        Task<Group> GetGroupById(Guid id);
        Task<IEnumerable<Group>> GetGroupByInstitute(Institute institute);
        Task<Group> GetGroupByName(string name);
        Task<Guid> UpdateGroup(Guid id, string name, Guid instituteId);
    }
}