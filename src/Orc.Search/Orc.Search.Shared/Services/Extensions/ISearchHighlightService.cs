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
    using Catel.Threading;

    public static class ISearchHighlightServiceExtensions
    {
        public static Task ResetHighlightsAsync(this ISearchHighlightService searchHighlightService)
        {
            Argument.IsNotNull(() => searchHighlightService);

            return TaskHelper.Run(() => searchHighlightService.ResetHighlights());
        }

        public static Task HighlightSearchablesAsync(this ISearchHighlightService searchHighlightService, IEnumerable<object> searchables)
        {
            Argument.IsNotNull(() => searchHighlightService);

            return TaskHelper.Run(() => searchHighlightService.HighlightSearchables(searchables));
        }
    }
}