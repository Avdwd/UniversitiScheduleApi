using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.DataAccess.Entities
{
    public class StudentProfileEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid GroupId { get; set; }
        public GroupEntity Group { get; set; } = null!;
    }
}
