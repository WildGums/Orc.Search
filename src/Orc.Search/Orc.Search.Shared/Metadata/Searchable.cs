// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Searchable.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Metadata;

    public class Searchable : ObjectWithMetadata, ISearchable
    {
        public Searchable(object instance, IMetadataCollection metadataCollection) 
            : base(instance, metadataCollection)
        {
        }
    }
}