
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Configurations;
using UNISchedule.DataAccess.Configurations.IdentityConfiguration;
using UNISchedule.DataAccess.Entities;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.DataAccess
{
    public class UniScheduleDbContext : IdentityDbContext<ApplicationUser> //inherited from DbContext
    {
        public UniScheduleDbContext(DbContextOptions<UniScheduleDbContext> options)
            :base(options)
        {
            
        }
        // DbSet properties for my entities
        public DbSet<ClassroomEntity> ClassroomEntities { get; set; }
        public DbSet<ClassTimeEntity> ClassTimeEntities { get; set; }
        public DbSet<GroupEntity> GroupEntities { get; set; }
        public DbSet<InstituteEntity> InstituteEntities { get; set; }
        public DbSet<ScheduleRecordEntity> ScheduleRecordEntities { get; set; }
        public DbSet<SubjectAssignmentEntity> SubjectAssignmentEntities { get; set; }
        public DbSet<SubjectEntity> SubjectEntities { get; set; }
        public DbSet<TypeSubjectEntity> TypeSubjectEntities { get; set; }
        // Identity entities
        public DbSet<ApplicationUser> ApplicationUserEntities { get; set; } = null!; // Initialize to avoid null reference
        public DbSet<StudentProfileEntity> StudentProfileEntities { get; set; }
        public DbSet<TeacherProfileEntity> TeacherProfileEntities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClassroomConfiguration());
            modelBuilder.ApplyConfiguration(new ClassTimeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new InstituteConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleRecordConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectAssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TypeSubjectConfiguration());
            // Apply configurations for Identity entities
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new StudentProfileConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherProfileConfiguration());
            // Apply other configurations here if needed

            

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniScheduleDbContext).Assembly);
            //// Add any additional configurations here if needed
        }
    }
}
