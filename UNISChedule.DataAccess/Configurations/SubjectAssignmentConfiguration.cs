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
    public class SubjectAssignmentConfiguration : IEntityTypeConfiguration<SubjectAssignmentEntity>
    {
        public void Configure(EntityTypeBuilder<SubjectAssignmentEntity> builder)
        {
            builder.HasKey(sa => sa.Id);

          
            builder.HasOne(sa => sa.ScheduleRecordEntitis)
                .WithOne(sr => sr.SubjectAssignmentEntity)
                .HasForeignKey<ScheduleRecordEntity>(sr => sr.SubjectAssignmentEntityId)
                .OnDelete(DeleteBehavior.Cascade); 

            
            builder.HasOne(sa => sa.GroupEntity)
                .WithMany(g => g.SubjectAssignments)
                .HasForeignKey(sa => sa.GroupEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(sa => sa.SubjectEntity)
                .WithMany()
                .HasForeignKey(sa => sa.SubjectEntityId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(sa => sa.TypeSubjectEntity)
                .WithMany()
                .HasForeignKey(sa => sa.TypeSubjectEntityID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
