// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Threading;

    public static class ISearchServiceExtensions
    {
        public static Task AddObjectsAsync(this ISearchService searchService, IEnumerable<ISearchable> searchables)
        {
            return TaskHelper.Run(() => searchService.AddObjects(searchables));
        }

        public static Task RemoveObjectsAsync(this ISearchService searchService, IEnumerable<ISearchable> searchables)
        {
            return TaskHelper.Run(() => searchService.RemoveObjects(searchables));
        }

        public static Task<IEnumerable<ISearchable>> SearchAsync(this ISearchService searchService, string filter, int maxResults = SearchDefaults.DefaultResults)
        {
            return TaskHelper.Run(() => searchService.Search(filter, maxResults));
        }
    }
}