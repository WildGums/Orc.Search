// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.Services
{
    using System;
    using System.Collections.Generic;
    using Models;
    using RandomDataGenerator.FieldOptions;
    using RandomDataGenerator.Randomizers;

    public class DataGenerationService : IDataGenerationService
    {
        private readonly Random _random = new Random();
        private readonly RandomizerFirstName _maleFirstNameRandomizer = new RandomizerFirstName(new FieldOptionsFirstName
        {
            Male = true
        });
        private readonly RandomizerFirstName _femaleFirstNameRandomizer = new RandomizerFirstName(new FieldOptionsFirstName
        {
            Female = true
        });
        private readonly RandomizerLastName _lastNameRandomizer = new RandomizerLastName(new FieldOptionsLastName
        {
           
        });

        public IEnumerable<ISearchable> GenerateSearchables(int objectCount = ExampleDefaults.GeneratedObjectCount)
        {
            var objects = new List<ISearchable>();

            for (var i = 0; i < objectCount; i++)
            {
                objects.Add(GenerateSearchable());
            }

            return objects;
        }

        public ISearchable GenerateSearchable()
        {
            var person = new Person();

            var isMale = _random.Next(1) == 0 ? true : false;
            if (isMale)
            {
                person.FirstName = _maleFirstNameRandomizer.Generate();
            }
            else
            {
                person.FirstName = _femaleFirstNameRandomizer.Generate();
            }

            person.LastName = _lastNameRandomizer.Generate();
            person.Age = _random.Next(18, 65);

            return GenerateSearchable(person);
        }

        public ISearchable GenerateSearchable(Person person)
        {
            return new ReflectionSearchable(person);
        }
    }
}
