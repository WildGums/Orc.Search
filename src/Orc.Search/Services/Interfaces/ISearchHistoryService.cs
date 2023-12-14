namespace Orc.Search
{
    using System.Collections.Generic;

    public interface ISearchHistoryService
    {
        IEnumerable<string> GetLastSearchQueries(string prefix, int count = 5);
    }
}
