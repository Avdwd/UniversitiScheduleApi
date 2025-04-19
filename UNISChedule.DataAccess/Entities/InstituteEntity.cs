

namespace UNISchedule.DataAccess.Entities
{
    public class InstituteEntity
    {
       
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<GroupEntity> Groups { get; set; } = [];
    }
}
