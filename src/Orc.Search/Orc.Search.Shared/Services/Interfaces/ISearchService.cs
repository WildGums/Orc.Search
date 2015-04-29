// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;

    public interface ISearchService
    {
        int IndexedObjectCount { get; }

        event EventHandler<EventArgs> Updating;
        event EventHandler<EventArgs> Updated;

        event EventHandler<SearchEventArgs> Searching;
        event EventHandler<SearchEventArgs> Searched;

        void AddObjects(IEnumerable<ISearchable> searchables);
        void RemoveObjects(IEnumerable<ISearchable> searchables);

        IEnumerable<object> Search(string filter, int maxResults = SearchDefaults.DefaultResults);
        IEnumerable<ISearchableMetadata> GetSearchableMetadata();
    }
}