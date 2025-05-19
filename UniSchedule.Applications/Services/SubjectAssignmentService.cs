using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;
//ToDo: дописати сюди викладача 
namespace UNISchedule.Applications.Services
{
    public class SubjectAssignmentService : ISubjectAssignmentService
    {
        private readonly ISubjectAssignmentRepository _subjectAssignmentRepository;
        public SubjectAssignmentService(ISubjectAssignmentRepository subjectAssignmentRepository)
        {
            _subjectAssignmentRepository = subjectAssignmentRepository;
        }
        //Method to create a subject assignment
        public async Task<Guid> CreateSubjectAssignment(SubjectAssignment subjectAssignment)
        {
            return await _subjectAssignmentRepository.Create(subjectAssignment);
        }
        //Method to delete a subject assignment
        public async Task<Guid> DeleteSubjectAssignment(Guid id)
        {
            return await _subjectAssignmentRepository.Delete(id);
        }
        //Method to get all subject assignments
        public async Task<IEnumerable<SubjectAssignment>> GetAllSubjectAssignments()
        {
            return await _subjectAssignmentRepository.Get();
        }

        //Method to update a subject assignment
        public async Task<Guid> UpdateSubjectAssignment(Guid id, Guid scheduleRecordId, Guid groupId, Guid subjectId, Guid typeSubjectId,string teacherProfileId)
        {
            return await _subjectAssignmentRepository.Update(id, scheduleRecordId, groupId, subjectId, typeSubjectId, teacherProfileId);
        }
        //Method to get a subject assignment by id
        public async Task<SubjectAssignment> GetSubjectAssignmentById(Guid id)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Id == id);
        }
        // Method to get a subject assignment by schedule record
        public async Task<SubjectAssignment> GetSubjectAssignmentByScheduleRecord(ScheduleRecord scheduleRecord)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.ScheduleRecord == scheduleRecord);
        }
        // Method to get a subject assignment by group
        public async Task<SubjectAssignment> GetSubjectAssignmentByGroup(Group group)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Group == group);
        }
        // Method to get a subject assignment by subject
        public async Task<SubjectAssignment> GetSubjectAssignmentBySubject(Subject subject)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Subject == subject);
        }
        // Method to get a subject assignment by type subject
        public async Task<SubjectAssignment> GetSubjectAssignmentByTypeSubject(TypeSubject typeSubject)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.TypeSubject == typeSubject);
        }
        // Method pagination
        public async Task<IEnumerable<SubjectAssignment>> GetSubjectAssignments(int pageNumber, int pageSize)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
        // Method to get a subject assignment by group and schedule record
        public async Task<SubjectAssignment> GetSubjectAssignmentByGroupAndScheduleRecord(Group group, ScheduleRecord scheduleRecord)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Group == group && c.ScheduleRecord == scheduleRecord);
        }
        // Method to get a subject assignment by group and subject
        public async Task<SubjectAssignment> GetSubjectAssignmentByGroupAndSubject(Group group, Subject subject)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Group == group && c.Subject == subject);
        }
        // Method to get a subject assignment by group and type subject
        public async Task<SubjectAssignment> GetSubjectAssignmentByGroupAndTypeSubject(Group group, TypeSubject typeSubject)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Group == group && c.TypeSubject == typeSubject);
        }
        //Method to get a subject assingment by group and teacher profile
        public async Task<SubjectAssignment> GetSubjectAssignmentByGroupAndTeacherProfile(Group group, TeacherProfile teacherProfile)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Group == group && c.Teacher == teacherProfile);
        }

        //Method to get a subject assignment by teacher profile
        public async Task<SubjectAssignment> GetSubjectAssignmentByTeacherProfile(TeacherProfile teacherProfile)
        {
            var subjectAssignments = await _subjectAssignmentRepository.Get();
            return subjectAssignments.FirstOrDefault(c => c.Teacher == teacherProfile);
        }
    }
}
