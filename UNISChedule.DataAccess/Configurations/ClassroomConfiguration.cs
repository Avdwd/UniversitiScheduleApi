

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Configurations
{
    public class ClassroomConfiguration : IEntityTypeConfiguration<ClassroomEntity>
    {
        public void Configure(EntityTypeBuilder<ClassroomEntity> builder)
        {
            builder.HasKey(c => c.Id);  

            builder.HasMany(c => c.ScheduleRecordEntities)
                .WithOne(sr => sr.ClassroomEntity)
                .HasForeignKey(sr => sr.ClassroomEntityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
