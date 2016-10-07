// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHistory.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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