

namespace UNISchedule.Core.Models
{
    public class SubjectAssignment
    {
        private SubjectAssignment(Guid id, ScheduleRecord scheduleRecord, Group group, Subject subject, TypeSubject typeSubject, TeacherProfile teacher)
        {
            Id = id;
            ScheduleRecord = scheduleRecord;
            Group = group;
            Subject = subject;
            TypeSubject = typeSubject;
            Teacher = teacher;
        }

        public Guid Id { get; }
        public ScheduleRecord ScheduleRecord { get; }
        public Group Group { get; }
        public Subject Subject { get; }
        // Teacher is added
        public TeacherProfile Teacher { get; }
        public TypeSubject TypeSubject { get; }


        public static(SubjectAssignment subjectAssignment,string error)Create(Guid id, ScheduleRecord scheduleRecord, Group group, Subject subject, TypeSubject typeSubject, TeacherProfile teacherProfile)
        {
            var error = string.Empty;
            // перевірка на null
            if (scheduleRecord == null)
            {
                error = "ScheduleRecord cannot be null.";
                return (null, error);
            }

            if (group == null)
            {
                error = "Group cannot be null.";
                return (null, error);
            }

            if (subject == null)
            {
                error = "Subject cannot be null.";
                return (null, error);
            }

            if (typeSubject == null)
            {
                error = "TypeSubject cannot be null.";
                return (null, error);
            }
            if(teacherProfile == null)
            {
                error = "Teacher cannot be null.";
                return (null, error);
            }

            var subjectAssignment = new SubjectAssignment(id, scheduleRecord, group, subject, typeSubject,teacherProfile);
            return (subjectAssignment, error); 
        }
    }
}
