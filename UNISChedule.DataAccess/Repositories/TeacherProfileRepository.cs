using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.DataAccess.Repositories
{
    public class TeacherProfileRepository
    {
        private readonly UniScheduleDbContext _context;
        public TeacherProfileRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherProfile>> Get()
        {
            var teacherProfileEntities = await _context.TeacherProfileEntities
                .AsNoTracking()
                .Include(t => t.ApplicationUser)
                .Include(t => t.Institute)
                .ToListAsync();

            var teacherProfiles = teacherProfileEntities
                .Select(t =>
                {
                    var applicationUserId = t.ApplicationUser.Id;
                    var details = UserDetails.Create(t.ApplicationUser.Id, t.ApplicationUser.UserName, t.ApplicationUser.FirstName, t.ApplicationUser.LastName, t.ApplicationUser.Patronymic).userDatails;
                    var institute = Institute.Create(t.Institute.Id, t.Institute.Name).institute;
                    return TeacherProfile.Create(applicationUserId, institute, details).teacher;
                })
                .ToList();
            return teacherProfiles;
        }

        public async Task<Guid> Create(TeacherProfile teacherProfile)
        {
            var teacherProfileEntity = new TeacherProfileEntity
            {
                Id = teacherProfile.Id,
                InstituteId = teacherProfile.Institute.Id,
                ApplicationUserId = teacherProfile.UserDetails.Id
            };
            await _context.TeacherProfileEntities.AddAsync(teacherProfileEntity);
            await _context.SaveChangesAsync();
            return teacherProfileEntity.Id;
        }
    }
}
