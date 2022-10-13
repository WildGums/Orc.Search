namespace Orc.Search.Example.Models
{
    public class Person
    {
        [SearchableProperty(SearchName = "firstname")]
        public string FirstName { get; set; }

        [SearchableProperty(SearchName = "lastname")]
        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
