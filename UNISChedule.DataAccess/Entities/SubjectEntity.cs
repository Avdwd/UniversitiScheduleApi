

namespace UNISchedule.DataAccess.Entities
{
    public class SubjectEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<SubjectAssignmentEntity> SubjectAssignmentEntities { get; set; }

    }
}
