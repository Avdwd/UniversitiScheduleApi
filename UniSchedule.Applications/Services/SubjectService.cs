

using UNISchedule.Core.Interfaces;
using UNISchedule.Core.Models;

namespace UNISchedule.Applications.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        //Method to create a subject
        public async Task<Guid> CreateSubject(Subject subject)
        {
            return await _subjectRepository.Create(subject);
        }
        //Method to delete a subject
        public async Task<Guid> DeleteSubject(Guid id)
        {
            return await _subjectRepository.Delete(id);
        }
        //Method to get all subjects
        public async Task<IEnumerable<Subject>> GetAllSubjects()
        {
            return await _subjectRepository.Get();
        }

        //Method to update a subject
        public async Task<Guid> UpdateSubject(Guid id, string name)
        {
            return await _subjectRepository.Update(id, name);
        }
        //Method to get a subject by id
        public async Task<Subject> GetSubjectById(Guid id)
        {
            var subjects = await _subjectRepository.Get();
            return subjects.FirstOrDefault(c => c.Id == id);
        }
        // Method to get a subject by name
        public async Task<Subject> GetSubjectByName(string name)
        {
            var subjects = await _subjectRepository.Get();
            return subjects.FirstOrDefault(c => c.Name == name);
        }
        //Method pagination
        public async Task<IEnumerable<Subject>> GetSubjects(int pageNumber, int pageSize)
        {
            var subjects = await _subjectRepository.Get();
            return subjects.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
