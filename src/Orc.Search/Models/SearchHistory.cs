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
