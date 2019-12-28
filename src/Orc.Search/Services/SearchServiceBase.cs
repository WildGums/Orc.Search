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
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.QueryParsers.Classic;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Orc.Metadata;

    public abstract class SearchServiceBase : ISearchService
    {
        #region Constants
        private const string IndexId = "__index_id";
        #endregion

        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lockObject = new object();

        private readonly ISearchQueryService _searchQueryService;

        private int _currentIndex = 0;
        private readonly Dictionary<int, ISearchable> _indexedObjects = new Dictionary<int, ISearchable>();
        private readonly Dictionary<ISearchable, int> _searchableIndexes = new Dictionary<ISearchable, int>();
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

            Updating?.Invoke(this, EventArgs.Empty);

            lock (_lockObject)
            {
                using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                {
                    using (var writer = new IndexWriter(_indexDirectory, CreateIndexWriterConfig(analyzer)))
                    {
                        foreach (var searchable in searchables)
                        {
                            var index = _currentIndex++;
                            _indexedObjects.Add(index, searchable);
                            _searchableIndexes.Add(searchable, index);

                            var document = new Document();

                            var indexField = new StringField(IndexId, index.ToString(), Field.Store.YES);

                            document.Add(indexField);

                            var metadata = searchable.MetadataCollection;
                            var searchableMetadatas = metadata.All.OfType<ISearchableMetadata>();

                            foreach (var searchableMetadata in searchableMetadatas)
                            {
                                if (searchableMetadata.GetValue<object>(searchable.Instance, out var searchableMetadataValue))
                                {
                                    var searchableMetadataValueAsString = ObjectToStringHelper.ToString(searchableMetadataValue);

                                    var field = new TextField(searchableMetadata.SearchName, searchableMetadataValueAsString, Field.Store.YES);
                                    document.Add(field);

                                    if (!_searchableMetadata.ContainsKey(searchableMetadata.SearchName))
                                    {
                                        _searchableMetadata.Add(searchableMetadata.SearchName, searchableMetadata);
                                    }
                                }
                            }

                            writer.AddDocument(document);
                        }

                        writer.PrepareCommit();
                        writer.Commit();
                    }
                }
            }

            Updated?.Invoke(this, EventArgs.Empty);
        }

        public virtual void RemoveObjects(IEnumerable<ISearchable> searchables)
        {
            Initialize();

            lock (_lockObject)
            {
                Updating?.Invoke(this, EventArgs.Empty);

                using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                {
                    using (var writer = new IndexWriter(_indexDirectory, CreateIndexWriterConfig(analyzer)))
                    {
                        foreach (var searchable in searchables)
                        {
                            int index;
                            if (!_searchableIndexes.TryGetValue(searchable, out index))
                            {
                                continue;
                            }

                            var queryAsText = $"{IndexId}:{index}";
                            var parser = new QueryParser(LuceneDefaults.Version, string.Empty, analyzer);
                            var query = parser.Parse(queryAsText);

                            writer.DeleteDocuments(query);

                            _searchableIndexes.Remove(searchable);
                            _indexedObjects.Remove(index);
                        }

                        writer.PrepareCommit();
                        writer.Commit();
                    }
                }

                Updated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ClearAllObjects()
        {
            Initialize();

            Updating?.Invoke(this, EventArgs.Empty);

            lock (_lockObject)
            {
                using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                {
                    using (var writer = new IndexWriter(_indexDirectory, CreateIndexWriterConfig(analyzer)))
                    {
                        _indexedObjects.Clear();
                        _searchableMetadata.Clear();

                        writer.DeleteAll();

                        writer.PrepareCommit();
                        writer.Commit();
                    }
                }
            }

            Updated?.Invoke(this, EventArgs.Empty);
        }

        public virtual IEnumerable<ISearchable> Search(string filter, int maxResults = SearchDefaults.DefaultResults)
        {
            Initialize();

            var results = new List<ISearchable>();

            lock (_lockObject)
            {
                try
                {
                    Searching?.Invoke(this, new SearchEventArgs(filter, results));

                    Query finalQuery = null;

                    // Note: There are two issues with using regex here
                    //       1. Lucene uses lower case interpretation of each string for indexing.
                    //          That means in regular expression we can use only lower case characters
                    //       2. escape sequences do not work. Not sure why
                    //      
                    //       In order to fix (1), we have to force Lucene to index differently. Probably we need to have two 
                    //       versions if indeces. One for regular search and another for regex

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (filter.IsValidOrcSearchFilter())
                    {
                        using (var analyzer = new StandardAnalyzer(LuceneDefaults.Version))
                        {
                            var queryAsText = _searchQueryService.GetSearchQuery(filter, GetSearchableMetadata());

                            var parser = new QueryParser(LuceneDefaults.Version, string.Empty, analyzer);
                            finalQuery = parser.Parse(queryAsText);
                        }
                    }

                    if (finalQuery != null)
                    {
                        using (var indexReader = DirectoryReader.Open(_indexDirectory))
                        {
                            var searcher = new IndexSearcher(indexReader);

                            var search = searcher.Search(finalQuery, maxResults);
                            foreach (var scoreDoc in search.ScoreDocs)
                            {
                                var docId = scoreDoc.Doc;
                                var doc = searcher.Doc(docId);

                                var index = int.Parse(doc.Get(IndexId));
                                results.Add(_indexedObjects[index]);
                            }
                        }
                    }
                }
                catch (ParseException ex)
                {
                    Log.Warning(ex, "Failed to parse search pattern");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while searching, returning default results");
                }
                finally
                {
                    Searched?.Invoke(this, new SearchEventArgs(filter, results));
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
                using (var indexWriter = new IndexWriter(_indexDirectory, CreateIndexWriterConfig(analyzer)))
                {
                    indexWriter.Commit();
                }
            }
        }

        protected virtual IndexWriterConfig CreateIndexWriterConfig(Analyzer analyzer)
        {
            var config = new IndexWriterConfig(LuceneDefaults.Version, analyzer)
            {

            };

            return config;
        }

        protected abstract Directory GetDirectory();
        #endregion
    }
}
