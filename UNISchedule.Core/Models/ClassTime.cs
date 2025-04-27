
using System.Globalization;


namespace UNISchedule.Core.Models
{
    public class ClassTime
    {
        public const int MAX_TIMEFRAME_LENGHT = 100;
        private ClassTime(Guid id, string timeframe)
        {
            Id = id;
            Timeframe = timeframe;
        }
        public Guid Id { get; }
        public string Timeframe { get; } = string.Empty;

        public static (ClassTime classTime, string error) Create(Guid id, string timeframe)
        {
            var error = string.Empty;

            //if (IsValidTimeRange(timeframe))
            //{
            //    error = "Invalid. Must be digit in format 00:00 - 00:00"; //цю частину треба допилити 
            //}

            var classTime = new ClassTime(id, timeframe);

            return (classTime, error);
        }



        //ToDo: виправити перевірку часу 

        // Перевірка на правильність часу
        public static bool IsValidTimeRange(string timeRangeString)
        {
            if (string.IsNullOrWhiteSpace(timeRangeString))
                return false;

            string[] parts = timeRangeString.Split(new[] { '-' }, 2);
            //перевіряємо, чи отримали рівно дві частини
            if (parts.Length != 2)
                return false;

            string startTimeString = parts[0].Trim();// Видаляємо зайві пробіли
            string endTimeString = parts[1].Trim();

            if (string.IsNullOrEmpty(startTimeString) || string.IsNullOrEmpty(endTimeString))
                return false;

            string[] timeFormats = { @"hh\:mm", @"h\:mm" };

            bool isStartTimeValid = TimeSpan.TryParseExact(
                startTimeString,
                timeFormats,
                CultureInfo.InvariantCulture,
                TimeSpanStyles.None,
                out TimeSpan startTime
            );

            bool isEndTimeValid = TimeSpan.TryParseExact(
                endTimeString,
                timeFormats,
                CultureInfo.InvariantCulture,
                TimeSpanStyles.None,
                out TimeSpan endTime
            );

            if (!isStartTimeValid || !isEndTimeValid)
                return false;

            //  часи мають бути в межах 05:00 - 23:00
            TimeSpan minTime = new TimeSpan(5, 0, 0);   // 05:00
            TimeSpan maxTime = new TimeSpan(23, 0, 0);  // 23:00

            bool isWithinAllowedRange = startTime >= minTime &&
                                        endTime <= maxTime &&
                                        startTime <= endTime; // Щоб не було 22:00 - 06:00

            return isWithinAllowedRange;
        }

    }
}
