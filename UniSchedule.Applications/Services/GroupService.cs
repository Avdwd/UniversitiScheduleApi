

using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;

namespace UNISchedule.Applications.Services
{
    public class GroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        //Method to create a group
        public async Task<Guid> CreateGroup(Group group)
        {
            return await _groupRepository.Create(group);
        }
        //Method to delete a group
        public async Task<Guid> DeleteGroup(Guid id)
        {
            return await _groupRepository.Delete(id);
        }
        //Method to get all groups
        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            return await _groupRepository.Get();
        }
        //Method to update a group
        public async Task<Guid> UpdateGroup(Guid id, string name, Guid instituteId)
        {
            return await _groupRepository.Update(id, name, instituteId);
        }
        //Method to get a group by id
        public async Task<Group> GetGroupById(Guid id)
        {
            var groups = await _groupRepository.Get();
            return groups.FirstOrDefault(c => c.Id == id);
        }
        // Method to get a group by name
        public async Task<Group> GetGroupByName(string name)
        {
            var groups = await _groupRepository.Get();
            return groups.FirstOrDefault(c => c.Name == name);
        }
        // Method to get a group by institute 
        public async Task<IEnumerable<Group>> GetGroupByInstitute(Institute institute)
        {
            var groups = await _groupRepository.Get();
            return groups.Where(c => c.Institute == institute);
        }

        //Method pagination
        public async Task<IEnumerable<Group>> GetClassrooms(int pageNumber, int pageSize)
        {
            var classrooms = await _groupRepository.Get();
            return classrooms.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

    }
}
