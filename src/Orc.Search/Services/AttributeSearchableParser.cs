// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeSearchableParser.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.Caching;
    using Catel.Reflection;

    public class AttributeSearchableParser : ISearchableParser
    {
        private readonly ICacheStorage<Type, List<SearchableMetadata>> _propertiesCache = new CacheStorage<Type, List<SearchableMetadata>>(); 

        public AttributeSearchableParser()
        {
            
        }

        public virtual IEnumerable<SearchableMetadata> GetSearchableMetadata(object searchable)
        {
            Argument.IsNotNull(() => searchable);

            var type = searchable.GetType();

            return _propertiesCache.GetFromCacheOrFetch(type, () =>
            {
                var searchableProperties = new List<SearchableMetadata>();

                var properties = type.GetPropertiesEx();
                foreach (var property in properties)
                {
                    var searchablePropertyAttribute = property.GetCustomAttributeEx(typeof (SearchablePropertyAttribute), false) as SearchablePropertyAttribute;
                    if (searchablePropertyAttribute != null)
                    {
                        var searchableProperty = new SearchableMetadata(property.Name);
                        if (!string.IsNullOrWhiteSpace(searchablePropertyAttribute.SearchName))
                        {
                            searchableProperty.SearchName = searchablePropertyAttribute.SearchName;
                        }
                        searchableProperty.Analyze = searchablePropertyAttribute.Analyze;

                        searchableProperties.Add(searchableProperty);
                    }
                }

                return searchableProperties;
            });
        }
    }
}