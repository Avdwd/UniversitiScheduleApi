

namespace UNISchedule.Core.Models
{
    public class TypeSubject
    {

        public const int MAX_TYPE_LENGTH = 100; 
        private TypeSubject(Guid id, string type)
        {
            Id = id;
            Type = type;
        }

        public Guid Id { get; }
        public string Type { get; } = string.Empty;

        public static (TypeSubject typeSubject, string error) Create(Guid id, string type)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(type) || type.Length > MAX_TYPE_LENGTH)
            {
                error = "Type cannot be empty";
            }

            var typeSubject = new TypeSubject(id, type);

            return (typeSubject, error);

        }

    }
}
