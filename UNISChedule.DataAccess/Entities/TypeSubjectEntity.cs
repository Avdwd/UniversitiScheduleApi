

namespace UNISchedule.DataAccess.Entities
{
    public class TypeSubjectEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public ICollection<SubjectAssignmentEntity> SubjectAssignmentEntities { get; set; }

    }
}
