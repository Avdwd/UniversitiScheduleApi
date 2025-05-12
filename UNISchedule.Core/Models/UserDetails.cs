using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNISchedule.Core.Models
{
    public class UserDetails
    {
        private UserDetails(string id, string userName, string lastName, string firstName, string patronymic  ) {
            Id = id;            
            UserName = userName;
            LastName = lastName;
            FirstName = firstName;
            Patronymic = patronymic;
        }
        public string Id { get; }
        public string UserName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;
        public string FirstName { get; } = string.Empty;
        public string Patronymic { get; } = string.Empty;

        public static (UserDetails userDatails, string error) Create(string id, string userName, string lastName, string firstName, string patronymic)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(id))
            {
                error = "Id cannot be empty or more then 255 characters";
            }
            if (string.IsNullOrEmpty(userName) || userName.Length > 255)
            {
                error = "UserName cannot be empty or more then 255 characters";
            }
            if (string.IsNullOrEmpty(lastName) || lastName.Length > 255)
            {
                error = "LastName cannot be empty or more then 255 characters";
            }
            if (string.IsNullOrEmpty(firstName) || firstName.Length > 255)
            {
                error = "FirstName cannot be empty or more then 255 characters";
            }
            if (string.IsNullOrEmpty(patronymic) || patronymic.Length > 255)
            {
                error = "Patronymic cannot be empty or more then 255 characters";
            }
            var userDatails = new UserDetails(id, userName, lastName, firstName, patronymic);
            return (userDatails, error);
        }
    }
}
