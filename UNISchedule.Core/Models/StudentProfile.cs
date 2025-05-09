using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNISchedule.Core.Models
{
    public class StudentProfile
    {
        private StudentProfile(string applicationUserId, Group group)
        {
            ApplicationUserId = applicationUserId;
            Group = group;
        }

        public string ApplicationUserId { get; } = string.Empty;
        public Group Group { get; } = null!;

        public static (StudentProfile student, string error) Create(string applicationUserId, Group group)
        {
            var error = string.Empty;
            if (string.IsNullOrWhiteSpace(applicationUserId))
            {
                error = "Can not be empty";
                return (null, error);
            }

            var student = new StudentProfile(applicationUserId, group);
            return (student, error);
        }
       


    }


}
