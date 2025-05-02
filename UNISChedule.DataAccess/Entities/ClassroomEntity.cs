

namespace UNISchedule.DataAccess.Entities
{
    public class ClassroomEntity
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int Building { get; set; }
        public ICollection<ScheduleRecordEntity> ScheduleRecordEntities { get; set; } = [];

    }
}
