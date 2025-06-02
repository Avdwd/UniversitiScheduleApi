namespace UniversitySchedule.UI.Models
{
    public class GroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public InstituteDto Institute { get; set; }
    }
}
