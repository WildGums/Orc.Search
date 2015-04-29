// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchQueryService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;

    public class SearchQueryService : ISearchQueryService
    {
        public string GetSearchQuery(string filter, IEnumerable<ISearchableMetadata> searchableMetadatas)
        {
            if (!filter.Contains(":"))
            {
                filter = filter.PrepareOrcSearchFilter();

                using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                {
                    var fields = new List<string>();
                    foreach (var searchableMetadata in searchableMetadatas)
                    {
                        fields.Add(searchableMetadata.SearchName);
                    }

                    var parser = new MultiFieldQueryParser(LuceneDefaults.Version, fields.ToArray(), analyzer);
                    var query  = parser.Parse(filter);
                    filter = query.ToString();
                }
            }

            return filter;
        }

        public string GetSearchQuery(params ISearchableMetadataValue[] searchableMetadataValues)
        {
            var query = new PhraseQuery();

            foreach (var searchableMetadataValue in searchableMetadataValues)
            {
                query.Add(new Term(searchableMetadataValue.Metadata.SearchName, searchableMetadataValue.Value));
            }

            var filter = query.ToString();

            return GetSearchQuery(filter, new SearchableMetadata[] { });
        }
    }
}