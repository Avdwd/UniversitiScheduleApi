using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNISchedule.DataAccess.Entities;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.DataAccess.Configurations.IdentityConfiguration
{
    public class ApplicationUserConfiguration: IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Configure properties for ApplicationUser here
            // For example:
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Patronymic)
                .IsRequired()
                .HasMaxLength(50);

            builder.
                HasOne(u => u.StudentProfile)
                .WithOne(sp => sp.ApplicationUser)
                .HasForeignKey<StudentProfileEntity>(sp => sp.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.
                HasOne(u => u.TeacherProfile)
                .WithOne(tp => tp.ApplicationUser)
                .HasForeignKey<TeacherProfileEntity>(tp => tp.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
