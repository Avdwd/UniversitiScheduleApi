

namespace UNISchedule.Core.Models
{
    public class ScheduleRecord
    {
        public const int MAX_ADDITIONAL_DATA_LENGHT = 255;

        private ScheduleRecord(Guid id, DateOnly date, string additionalData, ClassTime classTime, Classroom classroom)
        {
            Id = id;
            Date = date;
            AdditionalData = additionalData;
            ClassTime = classTime;
            Classroom = classroom;
        }

        public Guid Id { get;}
        public DateOnly Date { get;}
        public string AdditionalData { get;} = string.Empty;
        public ClassTime ClassTime { get; }
        public Classroom Classroom { get; }


        public static (ScheduleRecord scheduleRecord, string error) Create(Guid id, DateOnly date, string additionalData, ClassTime classTime, Classroom classroom)
        {
            var error = string.Empty;
            //перевірка на правильність заповнення  
            if (string.IsNullOrWhiteSpace(additionalData))
            {
                error = "Can not be empty";
                return (null, error);
            }

            if (additionalData.Length > MAX_ADDITIONAL_DATA_LENGHT)
            {
                error = $"Additional data exceeds the maximum length of {MAX_ADDITIONAL_DATA_LENGHT} characters.";
                return (null, error);
            }

            if (classTime == null)
            {
                error = "ClassTime must not be null.";
                return (null, error);
            }

            if (classroom == null)
            {
                error = "Classroom must not be null.";
                return (null, error);
            }

            var scheduleRecord = new ScheduleRecord(id, date, additionalData, classTime, classroom);
            return (scheduleRecord, error);
        }
    }
}
