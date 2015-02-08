// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchablePropertyValue.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Catel;

    public class SearchablePropertyValue
    {
        public SearchablePropertyValue(SearchableProperty searchableProperty)
        {
            Argument.IsNotNull(() => searchableProperty);

            SearchableProperty = searchableProperty;
        }

        public SearchableProperty SearchableProperty { get; private set; }

        public string Value { get; set; }
    }
}