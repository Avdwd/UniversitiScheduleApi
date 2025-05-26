using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNISchedule.Core.Models
{
    public record LoginResultDto(
        bool Succeeded, 
        bool IsLockedOut, 
        bool IsNotAllowed, 
        bool RequiresTwoFactor, 
        string? UserId = null);
}
