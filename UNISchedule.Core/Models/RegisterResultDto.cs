using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNISchedule.Core.Models
{
    public record RegisterResultDto(
        bool Succeeded,
        IEnumerable<string>? Errors = null,
        string? UserId = null);
}
