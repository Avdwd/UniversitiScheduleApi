using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Configurations
{
    public class ScheduleRecordConfiguration : IEntityTypeConfiguration<ScheduleRecordEntity>
    {
        public void Configure(EntityTypeBuilder<ScheduleRecordEntity> builder)
        {
            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Date)
                .IsRequired();

            builder.Property(sr => sr.AdditionalData)
                .HasMaxLength(500);

            builder.HasOne(sr => sr.ClassTime)
                .WithMany(ct => ct.ScheduleRecordEntities)
                .HasForeignKey(sr => sr.ClassTimeEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sr => sr.ClassroomEntity)
                .WithMany(c => c.ScheduleRecordEntities)
                .HasForeignKey(sr => sr.ClassroomEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-one з SubjectAssignmentEntity (налаштовано у SubjectAssignment)
        }
    }
}
