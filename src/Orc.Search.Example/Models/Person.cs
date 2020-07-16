// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.Models
{
    using System;

    public class Person
    {
        [SearchableProperty(SearchName = "firstname")]
        public string FirstName { get; set; }

        [SearchableProperty(SearchName = "lastname")]
        public string LastName { get; set; }

        public int Age { get; set; }
    }
}