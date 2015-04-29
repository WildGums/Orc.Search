// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchableMetadataValue.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public interface ISearchableMetadataValue
    {
        ISearchableMetadata Metadata { get; }
        string Value { get; }
    }
}