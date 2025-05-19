using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniSchedule.Applications.Services;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Applications.Services;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.DataAccess;
using UNISchedule.DataAccess.Entities.Identity;
using UNISchedule.DataAccess.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UniScheduleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Конфігурація ASP.NET Core Identity ---

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Налаштування вимог до паролю, блокування тощо
    options.SignIn.RequireConfirmedAccount = true; // Вимагати підтвердження email
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<UniScheduleDbContext>() // Вказуємо, що Identity буде зберігати дані у UniScheduleDbContext
.AddDefaultTokenProviders(); // Додає стандартні провайдери токенів



//repositories
builder.Services.AddScoped<ITypeSubjectRepository, TypeSubjectRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISubjectAssignmentRepository, SubjectAssignmentRepository>();
builder.Services.AddScoped<IScheduleRecordRepository, ScheduleRecordRepository>();
builder.Services.AddScoped<IClassTimeRepository, ClassTimeRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IInstituteRepository, InstituteRepository>();
builder.Services.AddScoped<ITeacherProfileRepository, TeacherProfileRepository>();
builder.Services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();
//services
builder.Services.AddScoped<ITypeSubjectService, TypeSubjectService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ISubjectAssignmentService, SubjectAssignmentService>();
builder.Services.AddScoped<IScheduleRecordService, ScheduleRecordService>();
builder.Services.AddScoped<IClassTimeService, ClassTimeService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IInstituteService, InstituteService>();
builder.Services.AddScoped<ITeacherProfileService, TeacherProfileService>();
builder.Services.AddScoped<IStudentProfileService, StudentProfileService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<UniScheduleDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Викликаємо ваш ініціалізатор
        await DBInitializer.Initialize(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during DB initialization.");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
