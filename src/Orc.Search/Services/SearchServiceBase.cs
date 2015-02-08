// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Version = Lucene.Net.Util.Version;

    public abstract class SearchServiceBase : ISearchService
    {
        #region Constants
        private const string IndexId = "__index_id";
        #endregion

        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lockObject = new object();

        private readonly ISearchQueryService _searchQueryService;
        private readonly ISearchableParser _searchableParser;
        private readonly ISearchableAdapter _searchableAdapter;

        private readonly Dictionary<int, object> _indexedObjects = new Dictionary<int, object>();

        private bool _initialized;

        private readonly Dictionary<string, SearchableProperty> _searchFields = new Dictionary<string, SearchableProperty>();
        private Lucene.Net.Store.Directory _indexDirectory;
        #endregion

        #region Constructors
        protected SearchServiceBase(ISearchQueryService searchQueryService, ISearchableParser searchableParser, ISearchableAdapter searchableAdapter)
        {
            Argument.IsNotNull(() => searchQueryService);
            Argument.IsNotNull(() => searchableParser);
            Argument.IsNotNull(() => searchableAdapter);

            _searchQueryService = searchQueryService;
            _searchableParser = searchableParser;
            _searchableAdapter = searchableAdapter;
        }
        #endregion

        #region Properties
        public int IndexedObjectCount
        {
            get
            {
                lock (_lockObject)
                {
                    return _indexedObjects.Count;
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs> Updating;

        public event EventHandler<EventArgs> Updated;

        public event EventHandler<EventArgs> Searching;

        public event EventHandler<SearchEventArgs> Searched;
        #endregion

        #region Methods
        public virtual IEnumerable<SearchableProperty> GetSearchableProperties()
        {
            lock (_lockObject)
            {
                var searchableProperties = new List<SearchableProperty>(_searchFields.Values);

                return searchableProperties;
            }
        }

        public virtual void AddObjects(IEnumerable<object> searchables)
        {
            Initialize();

            lock (_lockObject)
            {
                Updating.SafeInvoke(this);

                using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                {
                    using (var writer = new IndexWriter(_indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        foreach (var searchable in searchables)
                        {
                            var index = _indexedObjects.Count;
                            _indexedObjects.Add(index, searchable);

                            var document = new Document();
                            document.Add(new Field(IndexId, index.ToString(), Field.Store.YES, Field.Index.NO));

                            var searchableProperties = _searchableParser.GetSearchableProperties(searchable);
                            foreach (var searchableProperty in searchableProperties)
                            {
                                var searchablePropertyValue = _searchableAdapter.GetValue(searchable, searchableProperty);
                                var searchablePropertyValueAsString = ObjectToStringHelper.ToString(searchablePropertyValue);

                                var field = new Field(searchableProperty.Name, searchablePropertyValueAsString, Field.Store.YES, searchableProperty.Analyze ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED, Field.TermVector.NO);

                                document.Add(field);

                                if (!_searchFields.ContainsKey(searchableProperty.Name))
                                {
                                    _searchFields.Add(searchableProperty.Name, searchableProperty);
                                }
                            }

                            writer.AddDocument(document);
                        }

                        writer.Optimize();
                        writer.Commit();
                    }
                }

                Updated.SafeInvoke(this);
            }
        }

        public virtual void RemoveObjects(IEnumerable<object> searchables)
        {
            Initialize();

            throw new NotImplementedException("Not yet implemented");

            //lock (_lockObject)
            //{
            //    Updating.SafeInvoke(this);

            //    using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
            //    {
            //        using (var writer = new IndexWriter(_indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            //        {
            //            foreach (var searchable in searchables)
            //            {
            //                //writer.DeleteDocuments();
            //            }

            //            writer.Optimize();
            //            writer.Commit();
            //        }
            //    }

            //    Updated.SafeInvoke(this);
            //}
        }

        public virtual IEnumerable<object> Search(string filter, int maxResults = SearchDefaults.DefaultResults)
        {
            Initialize();

            var results = new List<object>();

            if (!filter.IsValidOrcSearchFilter())
            {
                return results;
            }

            lock (_lockObject)
            {
                try
                {
                    Searching.SafeInvoke(this);

                    using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                    {
                        var queryAsText = _searchQueryService.GetSearchQuery(filter, GetSearchableProperties());

                        var parser = new QueryParser(LuceneDefaults.Version, string.Empty, analyzer);
                        var query = parser.Parse(queryAsText);

                        using (var searcher = new IndexSearcher(_indexDirectory))
                        {
                            var search = searcher.Search(query, maxResults);
                            foreach (var scoreDoc in search.ScoreDocs)
                            {
                                var score = scoreDoc.Score;
                                var docId = scoreDoc.Doc;
                                var doc = searcher.Doc(docId);

                                var index = int.Parse(doc.Get(IndexId));
                                results.Add(_indexedObjects[index]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while searching, returning default reuslts");
                }
                finally
                {
                    Searched.SafeInvoke(this, new SearchEventArgs(filter, results));
                }
            }

            return results;
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            _indexDirectory = GetDirectory();

            // Required to create empty index, which is required for our reader
            using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
            {
                using (var indexWriter = new IndexWriter(_indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    indexWriter.Commit();
                }
            }
        }

        protected abstract Lucene.Net.Store.Directory GetDirectory();
        #endregion
    }
}