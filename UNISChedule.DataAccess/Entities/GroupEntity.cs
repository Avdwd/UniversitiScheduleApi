

namespace UNISchedule.DataAccess.Entities
{
    public class GroupEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid InstituteEntityId { get; set; } // Foreign key to InstituteEntity
        public InstituteEntity InstituteEntity { get; set; }
        public ICollection<StudentProfileEntity> StudentProfiles { get; set; } = [];
        public ICollection<SubjectAssignmentEntity> SubjectAssignments { get; set; } = [];
    }
}
