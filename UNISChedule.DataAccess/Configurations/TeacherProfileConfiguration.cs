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
    public class TeacherProfileConfiguration: IEntityTypeConfiguration<TeacherProfileEntity>
    {
        public void Configure(EntityTypeBuilder<TeacherProfileEntity> builder)
        {
            builder.HasKey(tp => tp.ApplicationUserId);

            builder.HasOne(tp => tp.ApplicationUser)
                .WithOne(u => u.TeacherProfile)
                .HasForeignKey<TeacherProfileEntity>(tp => tp.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tp => tp.Institute)
                .WithMany()
                .HasForeignKey(tp => tp.InstituteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(tp => tp.subjectAssignmentEntities)
                .WithOne(sa => sa.TeacherProfileEntity)
                .HasForeignKey(sa => sa.TeacherProfileEntityId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
    
    
}
