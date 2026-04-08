namespace Orc.Search;

using Lucene.Net.Store;
using Microsoft.Extensions.Logging;

public class InMemorySearchService : SearchServiceBase
{
    public InMemorySearchService(ILogger<InMemorySearchService> logger, ISearchQueryService searchQueryService)
        : base(logger, searchQueryService)
    {
    }

    protected override Directory GetDirectory()
    {
        return new RAMDirectory();
    }
}
