using Microsoft.EntityFrameworkCore;
using UniSchedule.Applications.Services;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Applications.Services;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.DataAccess;
using UNISchedule.DataAccess.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UniScheduleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//repositories
builder.Services.AddScoped<ITypeSubjectRepository, TypeSubjectRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISubjectAssignmentRepository, SubjectAssignmentRepository>();
builder.Services.AddScoped<IScheduleRecordRepository, ScheduleRecordRepository>();
builder.Services.AddScoped<IClassTimeRepository, ClassTimeRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
//services
builder.Services.AddScoped<ITypeSubjectService, TypeSubjectService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ISubjectAssignmentService, SubjectAssignmentService>();
builder.Services.AddScoped<IScheduleRecordService, ScheduleRecordService>();
builder.Services.AddScoped<IClassTimeService, ClassTimeService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IGroupService, GroupService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
