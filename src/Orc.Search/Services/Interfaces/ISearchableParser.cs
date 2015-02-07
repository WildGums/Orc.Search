// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchableParser.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;

    public interface ISearchableParser
    {
        IEnumerable<SearchableProperty> GetSearchableProperties(object searchable);
    }
}