using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;

namespace UniSchedule.Applications.Services
{
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;
        public ClassroomService(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }
        //Method to create a classroom
        public async Task<Guid> CreateClassroom(Classroom classroom)
        {
            return await _classroomRepository.Create(classroom);
        }
        //Method to delete a classroom
        public async Task<Guid> DeleteClassroom(Guid id)
        {
            return await _classroomRepository.Delete(id);
        }
        //Method to get all classrooms
        public async Task<IEnumerable<Classroom>> GetAllClassrooms()
        {
            return await _classroomRepository.Get();
        }
        //Method to update a classroom
        public async Task<Guid> UpdateClassroom(Guid id, int number, int building)
        {
            return await _classroomRepository.Update(id, number, building);
        }
        //Method to get a classroom by id
        public async Task<Classroom> GetClassroomById(Guid id)
        {
            var classrooms = await _classroomRepository.Get();
            return classrooms.FirstOrDefault(c => c.Id == id);

        }

        // Method to get a classroom by number
        public async Task<Classroom> GetClassroomByNumber(int number)
        {
            var classrooms = await _classroomRepository.Get();
            return classrooms.FirstOrDefault(c => c.Number == number);
        }

        // Method to get a classroom by building list
        public async Task<IEnumerable<Classroom>> GetClassroomByBuilding(int building)
        {
            var classrooms = await _classroomRepository.Get();
            return classrooms.Where(c => c.Building == building);

        }

        // Method pagination
        public async Task<IEnumerable<Classroom>> GetClassrooms(int pageNumber, int pageSize)
        {
            var classrooms = await _classroomRepository.Get();
            return classrooms.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

    }
}
