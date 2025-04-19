

namespace UNISchedule.Core.Models
{
    public class Subject
    {
        public const int MAX_NAME_SUBJECT_LENGHT = 255; 
        private Subject(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;


        public static (Subject subject, string error) Create(Guid id, string name)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name)||name.Length > MAX_NAME_SUBJECT_LENGHT)
            {
                error = "Invalid name. Name cannot be empty or longer than 255 characters.";
            }


            var subject = new Subject(id, name);

            return (subject, error);

        }
    }
}
