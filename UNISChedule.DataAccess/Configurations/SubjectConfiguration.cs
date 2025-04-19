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
    public class SubjectConfiguration : IEntityTypeConfiguration<SubjectEntity>
    {
        public void Configure(EntityTypeBuilder<SubjectEntity> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(255);
           
            builder
                .HasMany(s => s.SubjectAssignmentEntities)
                .WithOne(sa => sa.SubjectEntity)
                .HasForeignKey(sa => sa.SubjectEntityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
