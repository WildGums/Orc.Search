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
        public string GetSearchQuery(string filter, IEnumerable<SearchableProperty> searchableProperties)
        {
            if (!filter.Contains(":"))
            {
                using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                {
                    var fields = new List<string>();
                    foreach (var searchableProperty in searchableProperties)
                    {
                        fields.Add(searchableProperty.Name);
                    }

                    var parser = new MultiFieldQueryParser(LuceneDefaults.Version, fields.ToArray(), analyzer);
                    var query  = parser.Parse(filter);
                    filter = query.ToString();
                }
            }

            return filter;
        }

        public string GetSearchQuery(params SearchablePropertyValue[] searchablePropertyValues)
        {
            var query = new PhraseQuery();

            foreach (var searchablePropertyValue in searchablePropertyValues)
            {
                query.Add(new Term(searchablePropertyValue.SearchableProperty.Name, searchablePropertyValue.Value));
            }

            var filter = query.ToString();

            return GetSearchQuery(filter, new SearchableProperty[] { });
        }
    }
}