// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Searchable.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Metadata;

    public class Searchable : ISearchable
    {
        public Searchable(object instance, IMetadataCollection metadataCollection)
        {
            Instance = instance;
            MetadataCollection = metadataCollection;
        }

        public object Instance { get; private set; }

        public IMetadataCollection MetadataCollection { get; private set; }
    }
}