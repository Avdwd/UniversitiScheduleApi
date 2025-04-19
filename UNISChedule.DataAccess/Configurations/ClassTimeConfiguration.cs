

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Configurations
{
    public class ClassTimeConfiguration : IEntityTypeConfiguration<ClassTimeEntity>
    {
        public void Configure(EntityTypeBuilder<ClassTimeEntity> builder)
        {
            builder
                .HasKey(ct => ct.Id);

            builder
                .Property(ct => ct.Timeframe)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasMany(ct => ct.ScheduleRecordEntities)
                .WithOne(sr => sr.ClassTime)
                .HasForeignKey(sr => sr.ClassTimeEntityId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
