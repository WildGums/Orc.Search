// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Searchable.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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