

using Microsoft.AspNetCore.Identity;

namespace UNISchedule.DataAccess.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Властивості, успадковані від IdentityUser:
        // Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
        // PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
        // TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount

        // Додаткові властивості:
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;

       
        
    }
}
