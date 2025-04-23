using UNISchedule.Core.Models;

namespace UniSchedule.Core.Interfaces.ServiceInterfaces
{
    public interface IClassroomService
    {
        Task<Guid> CreateClassroom(Classroom classroom);
        Task<Guid> DeleteClassroom(Guid id);
        Task<IEnumerable<Classroom>> GetAllClassrooms();
        Task<IEnumerable<Classroom>> GetClassroomByBuilding(int building);
        Task<Classroom> GetClassroomById(Guid id);
        Task<Classroom> GetClassroomByNumber(int number);
        Task<IEnumerable<Classroom>> GetClassrooms(int pageNumber, int pageSize);
        Task<Guid> UpdateClassroom(Guid id, int number, int building);
    }
}