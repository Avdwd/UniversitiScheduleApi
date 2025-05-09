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
            builder.HasKey(tp => tp.Id);
            builder.Property(tp => tp.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(tp => tp.LastName)
            
           
        }
    }
    
    
}
