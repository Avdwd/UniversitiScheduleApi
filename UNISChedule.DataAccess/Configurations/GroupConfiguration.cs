using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<GroupEntity>
    {
        public void Configure(EntityTypeBuilder<GroupEntity> builder)
        {
            builder.
                HasKey(g => g.Id);

            builder.
                Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.
                HasOne(g => g.InstituteEntity)
                .WithMany(i => i.Groups)
                .HasForeignKey(g => g.InstituteEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.StudentProfiles)
                .WithOne(sp => sp.Group)
                .HasForeignKey(sp => sp.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.
                HasMany(g => g.SubjectAssignments)
                .WithOne(sa => sa.GroupEntity)
                .HasForeignKey(sa => sa.GroupEntityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
