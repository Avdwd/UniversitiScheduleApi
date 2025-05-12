using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Configurations
{
    public class InstituteConfiguration : IEntityTypeConfiguration<InstituteEntity>
    {
        public void Configure(EntityTypeBuilder<InstituteEntity> builder)
        {
            builder
                .HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.
                HasMany(i => i.TeacherProfiles)
                .WithOne(tp => tp.Institute)
                .HasForeignKey(tp => tp.InstituteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(i => i.Groups)
                .WithOne(g => g.InstituteEntity)
                .HasForeignKey(g => g.InstituteEntityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
