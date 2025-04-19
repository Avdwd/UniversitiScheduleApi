

namespace UNISchedule.DataAccess.Entities
{
    public class SubjectAssignmentEntity
    {
        public Guid Id { get; set; }
        public Guid ScheduleRecordEntityId { get; set; }
        public ScheduleRecordEntity ScheduleRecordEntitis { get; set; }

        public Guid GroupEntityId { get; set; }
        public GroupEntity GroupEntity { get; set; }
        public Guid SubjectEntityId { get; set; }
        public SubjectEntity SubjectEntity { get; set; }
        //тут прописати ідшнік для преподаватєля
        //тут доступ до класу 
        public Guid TypeSubjectEntityID { get; set; }
        public TypeSubjectEntity TypeSubjectEntity { get; set; }

    }
}
