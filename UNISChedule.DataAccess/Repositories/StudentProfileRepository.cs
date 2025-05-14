using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class StudentProfileRepository : IStudentProfileRepository
    {
        private readonly UniScheduleDbContext _context;
        public StudentProfileRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentProfile>> Get()
        {
            var studentProfileEntities = await _context.StudentProfileEntities
                .AsNoTracking()
                .Include(s => s.ApplicationUser)
                .Include(s => s.Group)
                .ToListAsync();

            var studentProfiles = studentProfileEntities
                .Select(s =>
                {
                    var applicationUserId = s.ApplicationUser.Id;
                    var details = UserDetails.Create(s.ApplicationUser.Id, s.ApplicationUser.UserName, s.ApplicationUser.FirstName, s.ApplicationUser.LastName, s.ApplicationUser.Patronymic).userDatails;
                    var institute = Institute.Create(s.Group.InstituteEntity.Id, s.Group.InstituteEntity.Name).institute;
                    var group = Group.Create(s.Group.Id, s.Group.Name, institute).group;
                    return StudentProfile.Create(applicationUserId, group, details).student;
                })
                .ToList();
            return studentProfiles;
        }

        public async Task<string> Create(StudentProfile studentProfile)
        {
            var studentProfileEntity = new StudentProfileEntity
            {
                GroupId = studentProfile.Group.Id,
                ApplicationUserId = studentProfile.UserDetails.Id
            };
            await _context.StudentProfileEntities.AddAsync(studentProfileEntity);
            await _context.SaveChangesAsync();
            return studentProfileEntity.ApplicationUserId;
        }

        public async Task<string> Update(string id, string firstName, string lastName, string patronymic, Guid groupId)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.FirstName, u => firstName)
                    .SetProperty(u => u.LastName, u => lastName)
                    .SetProperty(u => u.Patronymic, u => patronymic));

            await _context.StudentProfileEntities
                .Where(s => s.ApplicationUserId == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.GroupId, s => groupId));
            return id;
        }

        public async Task<string> Delete(string id)
        {
            await _context.StudentProfileEntities
                .Where(s => s.ApplicationUserId == id)
                .ExecuteDeleteAsync();
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
