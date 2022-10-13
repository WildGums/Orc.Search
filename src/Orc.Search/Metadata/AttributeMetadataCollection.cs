namespace Orc.Search
{
    using System.Collections.Generic;
    using Catel.Caching;
    using Metadata;
    using System;
    using Catel;
    using Catel.Reflection;

    public class AttributeMetadataCollection : ReflectionMetadataCollection
    {
        private static readonly ICacheStorage<Type, List<SearchableMetadata>> _propertiesCache = new CacheStorage<Type, List<SearchableMetadata>>();

        private readonly Type _targetType;

        public AttributeMetadataCollection(Type targetType) 
            : base(targetType)
        {
            _targetType = targetType;
        }

        public override IEnumerable<IMetadata> All
        {
            get
            {
                return _propertiesCache.GetFromCacheOrFetch(_targetType, () =>
                {
                    var searchableProperties = new List<SearchableMetadata>();

                    var properties = _targetType.GetPropertiesEx();
                    foreach (var property in properties)
                    {
                        var searchablePropertyAttribute = property.GetCustomAttributeEx(typeof(SearchablePropertyAttribute), false) as SearchablePropertyAttribute;
                        if (searchablePropertyAttribute is not null)
                        {
                            var searchableProperty = new SearchableMetadata(property);
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
}
