using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;
using UNISchedule.DataAccess.Entities.Identity;

namespace UNISchedule.DataAccess.Repositories
{
    public class TeacherProfileRepository : ITeacherProfileRepository
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
                    var Id = t.Id;
                    var applicationUserId = t.ApplicationUser.Id;
                    var details = UserDetails.Create(t.ApplicationUser.Id, t.ApplicationUser.UserName, t.ApplicationUser.FirstName, t.ApplicationUser.LastName, t.ApplicationUser.Patronymic).userDatails;
                    var institute = Institute.Create(t.Institute.Id, t.Institute.Name).institute;
                    return TeacherProfile.Create(Id,applicationUserId, institute, details).teacher;
                })
                .ToList();
            return teacherProfiles;
        }

        public async Task<string> Create(TeacherProfile teacherProfile)
        {
            var teacherProfileEntity = new TeacherProfileEntity
            {
                InstituteId = teacherProfile.Institute.Id,
                ApplicationUserId = teacherProfile.UserDetails.Id

            };
            await _context.TeacherProfileEntities.AddAsync(teacherProfileEntity);
            await _context.SaveChangesAsync();
            return teacherProfileEntity.ApplicationUserId;
        }

        public async Task<string> Update(string id, string firstName, string lastName, string patronymic, Guid instituteId)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.FirstName, u => firstName)
                    .SetProperty(u => u.LastName, u => lastName)
                    .SetProperty(u => u.Patronymic, u => patronymic));

            await _context.TeacherProfileEntities
                .Where(t => t.ApplicationUserId == id)
                .ExecuteUpdateAsync(t => t
                    .SetProperty(t => t.InstituteId, t => instituteId));

            return id;
        }

        public async Task<string> Delete(string id)
        {
            await _context.TeacherProfileEntities
                .Where(t => t.ApplicationUserId == id)
                .ExecuteDeleteAsync();
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
