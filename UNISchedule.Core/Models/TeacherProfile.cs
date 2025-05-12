using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNISchedule.Core.Models
{
    public class TeacherProfile
    {
        private TeacherProfile(string applicationUserId, Institute institute, UserDetails userDetails)
        {
            ApplicationUserId = applicationUserId;
            Institute = institute;
            UserDetails = userDetails;
        }
        public UserDetails UserDetails { get; } = null!;
        public string ApplicationUserId { get; } = string.Empty;
        public Institute Institute { get; }

        public static(TeacherProfile teacher, string error)Create(string applicationUserId,Institute institute, UserDetails userDetails)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(applicationUserId))
            {
                error = "Can not be empty ";
                return (null, error);
            }

            var teacher = new TeacherProfile(applicationUserId, institute, userDetails);
            return (teacher, error);
        }


    }
}
