// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchableAdapter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Catel.Reflection;

    public class ReflectionSearchableAdapter : ISearchableAdapter
    {
        public virtual object GetValue(object searchable, SearchableMetadata searchableMetadata)
        {
            return PropertyHelper.GetPropertyValue(searchable, searchableMetadata.Name);
        }
    }
}