// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchableMetadataValue.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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