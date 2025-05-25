using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNISchedule.Core.Models
{
    public record UserDto
        (
        string Id,
        string Email,
        string UserName,
        string FirstName,
        string LastName,
        string Patronymic
        );
    
}
