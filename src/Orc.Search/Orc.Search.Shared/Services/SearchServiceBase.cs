// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;

    public abstract class SearchServiceBase : ISearchService
    {
        #region Constants
        private const string IndexId = "__index_id";
        #endregion

        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lockObject = new object();

        private readonly ISearchQueryService _searchQueryService;

        private readonly Dictionary<int, ISearchable> _indexedObjects = new Dictionary<int, ISearchable>();
        private readonly Dictionary<string, ISearchableMetadata> _searchableMetadata = new Dictionary<string, ISearchableMetadata>();

        private bool _initialized;

        private Directory _indexDirectory;
        #endregion

        #region Constructors
        protected SearchServiceBase(ISearchQueryService searchQueryService)
        {
            Argument.IsNotNull(() => searchQueryService);

            _searchQueryService = searchQueryService;
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

        public event EventHandler<SearchEventArgs> Searching;

        public event EventHandler<SearchEventArgs> Searched;
        #endregion

        #region Methods
        public virtual IEnumerable<ISearchableMetadata> GetSearchableMetadata()
        {
            lock (_lockObject)
            {
                var searchableMetadata = new List<ISearchableMetadata>(_searchableMetadata.Values);
                return searchableMetadata;
            }
        }

        public virtual void AddObjects(IEnumerable<ISearchable> searchables)
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

                            var metadata = searchable.MetadataCollection;
                            var searchableMetadatas = metadata.All.OfType<ISearchableMetadata>();

                            foreach (var searchableMetadata in searchableMetadatas)
                            {
                                var searchableMetadataValue = searchableMetadata.GetValue(searchable.Instance);
                                var searchableMetadataValueAsString = ObjectToStringHelper.ToString(searchableMetadataValue);

                                var field = new Field(searchableMetadata.SearchName, searchableMetadataValueAsString, Field.Store.YES, searchableMetadata.Analyze ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED, Field.TermVector.NO);

                                document.Add(field);

                                if (!_searchableMetadata.ContainsKey(searchableMetadata.SearchName))
                                {
                                    _searchableMetadata.Add(searchableMetadata.SearchName, searchableMetadata);
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

        public virtual void RemoveObjects(IEnumerable<ISearchable> searchables)
        {
            Initialize();

            throw new NotImplementedException("Not yet implemented");
        }

        public void ClearAllObjects()
        {
            Initialize();

            lock (_lockObject)
            {
                Updating.SafeInvoke(this);

                using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                {
                    using (var writer = new IndexWriter(_indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        _indexedObjects.Clear();
                        _searchableMetadata.Clear();

                        writer.DeleteAll();

                        writer.Optimize();
                        writer.Commit();
                    }
                }

                Updated.SafeInvoke(this);
            }
        }

        public virtual IEnumerable<ISearchable> Search(string filter, int maxResults = SearchDefaults.DefaultResults)
        {
            Initialize();

            var results = new List<ISearchable>();

            lock (_lockObject)
            {
                try
                {
                    Searching.SafeInvoke(this, new SearchEventArgs(filter, results));

                    if (filter.IsValidOrcSearchFilter())
                    {
                        using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                        {
                            var queryAsText = _searchQueryService.GetSearchQuery(filter, GetSearchableMetadata());

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

        protected abstract Directory GetDirectory();
        #endregion
    }
}