
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UNISchedule.DataAccess.Configurations;
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

        public DbSet<ClassroomEntity> ClassroomEntities { get; set; }
        public DbSet<ClassTimeEntity> ClassTimeEntities { get; set; }
        public DbSet<GroupEntity> GroupEntities { get; set; }
        public DbSet<InstituteEntity> InstituteEntities { get; set; }
        public DbSet<ScheduleRecordEntity> ScheduleRecordEntities { get; set; }
        public DbSet<SubjectAssignmentEntity> SubjectAssignmentEntities { get; set; }
        public DbSet<SubjectEntity> SubjectEntities { get; set; }
        public DbSet<TypeSubjectEntity> TypeSubjectEntities { get; set; }
        //ToDo:дописати сети для викладача та студента 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassroomConfiguration());
            modelBuilder.ApplyConfiguration(new ClassTimeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new InstituteConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleRecordConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectAssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TypeSubjectConfiguration());
            //ToDo:дописати конфігурації для студента та викладача 

            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniScheduleDbContext).Assembly);
            //// Add any additional configurations here if needed
        }
    }
}
