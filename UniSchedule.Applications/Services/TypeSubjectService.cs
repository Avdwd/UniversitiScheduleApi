

using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;

namespace UNISchedule.Applications.Services
{
    public class TypeSubjectService
    {
        private readonly ITypeSubjectRepository _typeSubjectRepository;
        public TypeSubjectService(ITypeSubjectRepository typeSubjectRepository)
        {
            _typeSubjectRepository = typeSubjectRepository;
        }
        //Method to create a type subject
        public async Task<Guid> CreateTypeSubject(TypeSubject typeSubject)
        {
            return await _typeSubjectRepository.Create(typeSubject);
        }
        //Method to delete a type subject
        public async Task<Guid> DeleteTypeSubject(Guid id)
        {
            return await _typeSubjectRepository.Delete(id);
        }
        //Method to get all type subjects
        public async Task<IEnumerable<TypeSubject>> GetAllTypeSubjects()
        {
            return await _typeSubjectRepository.Get();
        }
        //Method to update a type subject
        public async Task<Guid> UpdateTypeSubject(Guid id, string name)
        {
            return await _typeSubjectRepository.Update(id, name);
        }
        //Method to get a type subject by id
        public async Task<TypeSubject> GetTypeSubjectById(Guid id)
        {
            var typeSubjects = await _typeSubjectRepository.Get();
            return typeSubjects.FirstOrDefault(c => c.Id == id);
        }
        // Method to get a type subject by type
        public async Task<TypeSubject> GetTypeSubjectByName(string type)
        {
            var typeSubjects = await _typeSubjectRepository.Get();
            return typeSubjects.FirstOrDefault(c => c.Type == type);
        }
    }
}
