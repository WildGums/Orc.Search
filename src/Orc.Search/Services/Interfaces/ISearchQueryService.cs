namespace Orc.Search
{
    using System.Collections.Generic;

    public interface ISearchQueryService
    {
        string GetSearchQuery(string filter, IEnumerable<ISearchableMetadata> searchableMetadatas);
        string GetSearchQuery(params ISearchableMetadataValue[] searchableMetadataValues);
    }
}
