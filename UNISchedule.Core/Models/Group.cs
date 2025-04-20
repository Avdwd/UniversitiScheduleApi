
namespace UNISchedule.Core.Models
{
    public class Group
    {
        public const int MAX_NAME_LENGTH = 100;
        private Group(Guid id, string name, Institute institute)
        {
            Id = id;
            Name = name;
            Institute = institute;
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        
        public  Institute Institute { get; }
        public static (Group group, string error) Create(Guid id, string name, Institute institute)
        {
            var error = string.Empty;
            if(string.IsNullOrEmpty(name)|| name.Length > MAX_NAME_LENGTH)
            {
                error = "Name cannot be empty or more then 100 characters";
            }

            var group = new Group(id, name, institute);
            return (group, error);
        }


    }
}
