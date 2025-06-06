﻿using Microsoft.EntityFrameworkCore;
using UNISchedule.Core.Interfaces.RepositoryInterfaces;
using UNISchedule.Core.Models;
using UNISchedule.DataAccess.Entities;

namespace UNISchedule.DataAccess.Repositories
{
    public class SubjectAssignmentRepository : ISubjectAssignmentRepository
    {
        private readonly UniScheduleDbContext _context;
        public SubjectAssignmentRepository(UniScheduleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubjectAssignment>> Get()
        {
            var subjectAssignmentEntities = await _context.SubjectAssignmentEntities
                .AsNoTracking()
                .Include(s => s.SubjectEntity)
                .Include(s => s.GroupEntity)
                        .ThenInclude(g => g.InstituteEntity)
                .Include(s => s.ScheduleRecordEntitis)
                        .ThenInclude(sr => sr.ClassTime)
                .Include(s => s.ScheduleRecordEntitis)
                        .ThenInclude(sr => sr.ClassroomEntity)
                .Include(s => s.TypeSubjectEntity)
                .Include(s => s.TeacherProfileEntity)
                        .ThenInclude(t => t.ApplicationUser)
                .Include(s => s.TeacherProfileEntity)
                        .ThenInclude(t => t.Institute)
                .ToListAsync();

            var subjectAssignments = subjectAssignmentEntities.Select(s =>
            {
                var classTime = ClassTime.Create(
                    s.ScheduleRecordEntitis.ClassTime.Id,
                    s.ScheduleRecordEntitis.ClassTime.Timeframe).classTime;

                var classroom = Classroom.Create(
                    s.ScheduleRecordEntitis.ClassroomEntity.Id,
                    s.ScheduleRecordEntitis.ClassroomEntity.Building,
                    s.ScheduleRecordEntitis.ClassroomEntity.Number).classroom;

                var scheduleRecord = ScheduleRecord.Create(
                    s.ScheduleRecordEntitis.Id,
                    s.ScheduleRecordEntitis.Date,
                    s.ScheduleRecordEntitis.AdditionalData,
                    classTime,
                    classroom).scheduleRecord;

                var institute = Institute.Create(
                    s.GroupEntity.InstituteEntity.Id,
                    s.GroupEntity.InstituteEntity.Name).institute;

                var group = Group.Create(
                    s.GroupEntity.Id,
                    s.GroupEntity.Name,
                    institute).group;

                var subject = Subject.Create(
                    s.SubjectEntity.Id,
                    s.SubjectEntity.Name).subject;

                var typeSubject = TypeSubject.Create(
                    s.TypeSubjectEntity.Id,
                    s.TypeSubjectEntity.Type).typeSubject;

                var userDetails = UserDetails.Create(
                    s.TeacherProfileEntity.ApplicationUser.Id,
                    s.TeacherProfileEntity.ApplicationUser.UserName,
                    s.TeacherProfileEntity.ApplicationUser.FirstName,
                    s.TeacherProfileEntity.ApplicationUser.LastName,
                    s.TeacherProfileEntity.ApplicationUser.Patronymic).userDatails;

                var teacherInstitute = Institute.Create(
                    s.TeacherProfileEntity.Institute.Id,
                    s.TeacherProfileEntity.Institute.Name).institute;

                var teacher = TeacherProfile.Create(
                    s.TeacherProfileEntity.Id,
                    s.TeacherProfileEntity.ApplicationUserId,
                    teacherInstitute,
                    userDetails).teacher;

                return SubjectAssignment.Create(s.Id, scheduleRecord, group, subject, typeSubject,teacher).subjectAssignment;

            });

            return subjectAssignments;
        }

        public async Task<Guid> Create(SubjectAssignment subjectAssignment)
        {
            var subjectAssignmentEntity = new SubjectAssignmentEntity
            {
                Id = subjectAssignment.Id,
                ScheduleRecordEntityId = subjectAssignment.ScheduleRecord.Id,
                GroupEntityId = subjectAssignment.Group.Id,
                SubjectEntityId = subjectAssignment.Subject.Id,
                TypeSubjectEntityID = subjectAssignment.TypeSubject.Id,
                TeacherProfileEntityId = subjectAssignment.Teacher.ApplicationUserId
            };
            await _context.SubjectAssignmentEntities.AddAsync(subjectAssignmentEntity);
            await _context.SaveChangesAsync();
            return subjectAssignmentEntity.Id;
        }

        public async Task<Guid> Update(Guid id, Guid scheduleRecordId, Guid groupId, Guid subjectId, Guid typeSubjectId, string teacherId)
        {
            await _context.SubjectAssignmentEntities
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.ScheduleRecordEntityId, sr => scheduleRecordId)
                    .SetProperty(s => s.GroupEntityId, g => groupId)
                    .SetProperty(s => s.SubjectEntityId, su => subjectId)
                    .SetProperty(s => s.TypeSubjectEntityID, ts => typeSubjectId)
                    .SetProperty(s => s.TeacherProfileEntityId, tp => teacherId)
                    );

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.SubjectAssignmentEntities
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
