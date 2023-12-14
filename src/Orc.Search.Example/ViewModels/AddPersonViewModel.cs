namespace Orc.Search.Example.ViewModels
{
    using Catel.MVVM;
    using Models;

    public class AddPersonViewModel : ViewModelBase
    {
        public AddPersonViewModel()
        {
            Person = new Person
            {
                Age = 20
            };
        }

        public override string Title => "Add person";

        public Person Person { get; private set; }
    }
}
