namespace Orc.Search
{
    using System;
    using System.Collections.Generic;

    public class SearchEventArgs : EventArgs
    {
        public SearchEventArgs(string filter, IEnumerable<ISearchable> results)
        {
            ArgumentNullException.ThrowIfNull(filter);
            ArgumentNullException.ThrowIfNull(results);

            Filter = filter;
            Results = results;
        }

        public string Filter { get; private set; }

        public IEnumerable<ISearchable> Results { get; private set; }
    }
}
