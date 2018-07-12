// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchableMetadataValue.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public class SearchableMetadataValue : ISearchableMetadataValue
    {
        public SearchableMetadataValue(ISearchableMetadata metadata, string value)
        {
            Metadata = metadata;
            Value = value;
        }

        public ISearchableMetadata Metadata { get; private set; }

        public string Value { get; private set; }
    }
}