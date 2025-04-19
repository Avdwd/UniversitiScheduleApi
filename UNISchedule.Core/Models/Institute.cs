

namespace UNISchedule.Core.Models
{
    public class Institute
    {
        public const int MAX_NAME_LENGHT = 255;
        private Institute(Guid id, string name, ICollection<Group> groups)
        {
            Id = id;
            Name = name;
            Groups = groups;
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public ICollection<Group> Groups { get; }

        public static (Institute institute, string error) Create(Guid id, string name, ICollection<Group> groups)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGHT)
            {
                error = "Name cannot be empty or more then 255 characters";
            }

            var safeInstitute = groups ?? new List<Group>();

            var institute = new Institute(id, name,groups);
            return (institute, error);
        }
    }
}
