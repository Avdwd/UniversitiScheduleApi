

namespace UNISchedule.DataAccess.Entities
{
    public class ScheduleRecordEntity
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public string AdditionalData { get; set; } = string.Empty;

        public Guid ClassTimeEntityId { get; set; }
        public ClassTimeEntity ClassTime { get; set; }
        public Guid ClassroomEntityId { get; set; }
        public ClassroomEntity ClassroomEntity { get; set; }
        public Guid SubjectAssignmentEntityId { get; set; }
        public SubjectAssignmentEntity SubjectAssignmentEntity { get; set; }
    }
}
