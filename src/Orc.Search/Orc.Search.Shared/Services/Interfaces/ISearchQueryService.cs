// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchQueryService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;

    public interface ISearchQueryService
    {
        #region Methods
        string GetSearchQuery(string filter, IEnumerable<ISearchableMetadata> searchableMetadatas);
        string GetSearchQuery(params ISearchableMetadataValue[] searchableMetadataValues);
        #endregion
    }
}