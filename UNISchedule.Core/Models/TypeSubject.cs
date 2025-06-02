

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
            // Валідація вхідних даних
            if (string.IsNullOrWhiteSpace(type)) // Використовуйте String.IsNullOrWhiteSpace для обробки пробілів
            {
                return (null, "Type cannot be empty or consist only of whitespace."); // Повертаємо null і помилку
            }

            if (type.Length > MAX_TYPE_LENGTH)
            {
                return (null, $"Type cannot exceed {MAX_TYPE_LENGTH} characters."); // Повертаємо null і помилку
            }

            // Якщо валідація пройшла успішно, створюємо об'єкт
            var typeSubject = new TypeSubject(id, type);

            return (typeSubject, string.Empty);

        }

    }
}
