namespace Orc.Search
{
    using Lucene.Net.Store;

    public class InMemorySearchService : SearchServiceBase
    {
        public InMemorySearchService(ISearchQueryService searchQueryService)
            : base(searchQueryService)
        {
        }

        protected override Directory GetDirectory()
        {
            return new RAMDirectory();
        }
    }
}
