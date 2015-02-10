// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchableMetadataValue.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Catel;

    public class SearchableMetadataValue
    {
        public SearchableMetadataValue(SearchableMetadata searchableMetadata)
        {
            Argument.IsNotNull(() => searchableMetadata);

            SearchableMetadata = searchableMetadata;
        }

        public SearchableMetadata SearchableMetadata { get; private set; }

        public string Value { get; set; }
    }
}