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
    public class TypeSubjectConfiguration : IEntityTypeConfiguration<TypeSubjectEntity>
    {
        public void Configure(EntityTypeBuilder<TypeSubjectEntity> builder)
        {
            builder
                .HasKey(ts => ts.Id);

            builder.Property(ts => ts.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(ts => ts.SubjectAssignmentEntities)
                .WithOne(sa => sa.TypeSubjectEntity)
                .HasForeignKey(sa => sa.TypeSubjectEntityID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
