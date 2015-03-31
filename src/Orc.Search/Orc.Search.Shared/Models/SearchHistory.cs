// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHistory.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;
    using Catel.Data;

    public class SearchHistory : ModelBase
    {
        public SearchHistory()
        {
            SearchHistoryElements = new List<SearchHistoryElement>();
        }

        public List<SearchHistoryElement> SearchHistoryElements { get; private set; } 
    }
}