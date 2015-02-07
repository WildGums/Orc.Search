// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ISearchServiceExtensions
    {
        public static async Task AddObjectsAsync(this ISearchService searchService, IEnumerable<object> searchables)
        {
            await Task.Factory.StartNew(() => searchService.AddObjects(searchables));
        }

        public static async Task RemoveObjectsAsync(this ISearchService searchService, IEnumerable<object> searchables)
        {
            await Task.Factory.StartNew(() => searchService.RemoveObjects(searchables));
        }

        public static async Task<IEnumerable<object>> SearchAsync(this ISearchService searchService, string filter, int maxResults = SearchDefaults.DefaultResults)
        {
            return await Task.Factory.StartNew(() => searchService.Search(filter, maxResults));
        }
    }
}