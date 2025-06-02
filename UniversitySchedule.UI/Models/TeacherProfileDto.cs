using System.ComponentModel.DataAnnotations;

namespace UniversitySchedule.UI.Models
{
    public class TeacherProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public  InstituteDto Institute { get; set; }
        
    }
}
