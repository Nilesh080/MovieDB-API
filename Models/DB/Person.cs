using IMDBApi_Assignment4.Models.Enums;

namespace IMDBApi_Assignment4.Models.DB
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public DateOnly DOB { get; set; }

        public Gender Gender { get; set; }
    }
}
