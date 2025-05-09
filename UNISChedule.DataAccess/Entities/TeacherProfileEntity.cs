using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.DataAccess.Entities
{
    public class TeacherProfileEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid InstituteId { get; set; }
        public InstituteEntity Institute { get; set; } = null!;

        public ICollection<SubjectAssignmentEntity> subjectAssignmentEntities { get; set; }
    }
}
