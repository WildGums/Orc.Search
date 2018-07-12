// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddPersonViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.ViewModels
{
    using Catel.MVVM;
    using Models;

    public class AddPersonViewModel : ViewModelBase
    {
        #region Constructors
        public AddPersonViewModel()
        {
            Person = new Person {Age = 20};
        }
        #endregion

        #region Properties
        public override string Title => "Add person";

        public Person Person { get; private set; }
        #endregion
    }
}