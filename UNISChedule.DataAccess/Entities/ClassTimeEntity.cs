

namespace UNISchedule.DataAccess.Entities
{
    public class ClassTimeEntity
    {
        public Guid Id { get; set; }
        public string Timeframe { get; set; } = string.Empty;
        public ICollection<ScheduleRecordEntity> ScheduleRecordEntities { get; set; } = [];

    }
}
