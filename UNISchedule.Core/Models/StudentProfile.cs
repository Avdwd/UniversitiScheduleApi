using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNISchedule.Core.Models
{
    public class StudentProfile
    {
        private StudentProfile(string id, string applicationUserId, Group group, UserDetails userDetails)
        {
            Id = id;
            ApplicationUserId = applicationUserId;
            Group = group;
            UserDetails = userDetails;
        }
        public string Id { get; private set; }
        public UserDetails UserDetails { get; } = null!;
        public string ApplicationUserId { get; } = string.Empty;
        public Group Group { get; } = null!;

        public static (StudentProfile student, string error) Create(string id, string applicationUserId, Group group, UserDetails userDetails)
        {
            var error = string.Empty;
            if (string.IsNullOrWhiteSpace(applicationUserId))
            {
                error = "Can not be empty";
                return (null, error);
            }

            var student = new StudentProfile(id, applicationUserId, group, userDetails);
            return (student, error);
        }
       


    }


}
