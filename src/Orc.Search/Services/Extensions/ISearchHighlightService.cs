// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchHighlightService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;

    public static class ISearchHighlightServiceExtensions
    {
        public static async Task ResetHighlightsAsync(this ISearchHighlightService searchHighlightService)
        {
            Argument.IsNotNull(() => searchHighlightService);

            await Task.Factory.StartNew(() => searchHighlightService.ResetHighlights());
        }

        public static async Task HighlightSearchablesAsync(this ISearchHighlightService searchHighlightService, IEnumerable<object> searchables)
        {
            Argument.IsNotNull(() => searchHighlightService);

            await Task.Factory.StartNew(() => searchHighlightService.HighlightSearchables(searchables));
        }
    }
}