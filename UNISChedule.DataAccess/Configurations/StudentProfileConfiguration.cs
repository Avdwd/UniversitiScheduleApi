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
    public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfileEntity>
    {
        public void Configure(EntityTypeBuilder<StudentProfileEntity> builder)
        {
            builder.HasKey(sp => sp.ApplicationUserId);
            builder.HasOne(sp => sp.ApplicationUser)
                .WithOne()
                .HasForeignKey<StudentProfileEntity>(sp => sp.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(sp => sp.Group)
                .WithMany(g => g.StudentProfiles)
                .HasForeignKey(sp => sp.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
