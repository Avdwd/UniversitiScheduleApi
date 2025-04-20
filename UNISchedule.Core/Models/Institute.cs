

namespace UNISchedule.Core.Models
{
    public class Institute
    {
        public const int MAX_NAME_LENGHT = 255;
        private Institute(Guid id, string name)
        {
            Id = id;
            Name = name;
           
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        

        public static (Institute institute, string error) Create(Guid id, string name)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGHT)
            {
                error = "Name cannot be empty or more then 255 characters";
            }

           

            var institute = new Institute(id, name);
            return (institute, error);
        }
    }
}
