// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.Services
{
    using System;
    using System.Collections.Generic;
    using Models;
    using RandomNameGenerator;

    public class DataGenerationService : IDataGenerationService
    {
        private readonly Random _random = new Random();

        public IEnumerable<object> GenerateObjects(int objectCount = ExampleDefaults.GeneratedObjectCount)
        {
            var objects = new List<object>();

            for (var i = 0; i < objectCount; i++)
            {
                objects.Add(GenerateObject());
            }

            return objects;
        }

        public object GenerateObject()
        {
            var person = new Person();

            person.FirstName = NameGenerator.GenerateFirstName(Gender.Male);
            person.LastName = NameGenerator.GenerateLastName();
            person.Age = _random.Next(18, 65);

            return person;
        }
    }
}