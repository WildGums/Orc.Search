// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.Models
{
    public class Person
    {
        #region Properties
        [SearchableProperty(Name = "firstname")]
        public string FirstName { get; set; }

        [SearchableProperty(Name = "lastname")]
        public string LastName { get; set; }

        public int Age { get; set; }
        #endregion
    }
}