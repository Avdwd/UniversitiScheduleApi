

namespace UNISchedule.Core.Models
{
     public class Classroom
    {
        private Classroom(Guid id, int number, int building)
        {
            Id = id;
            Number = number;
            Building = building;
        }

        public Guid Id { get; }
        public int Number { get; }
        public int Building { get; }

        public static (Classroom classroom, string error)Create(Guid id, int number, int building)
        {
            var error = string.Empty;
            if (number <= 0 || building <= 0)
            {
                error = "Cannot be empty";
            }
            var classroom = new Classroom(id, number, building);
            return (classroom, error);
        }
    }
}
