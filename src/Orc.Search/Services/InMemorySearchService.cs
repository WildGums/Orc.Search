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

    public class InMemorySearchService : ISearchService
    {
        #region Constants
        private const string IndexId = "__index_id";
        #endregion

        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lockObject = new object();

        private readonly ISearchableParser _searchableParser;
        private readonly ISearchableAdapter _searchableAdapter;

        private readonly Dictionary<int, object> _indexedObjects = new Dictionary<int, object>();

        private bool _initialized;

        private readonly HashSet<string> _searchFields = new HashSet<string>();
        private RAMDirectory _indexDirectory;
        private IndexSearcher _indexSearcher;
        #endregion

        #region Constructors
        public InMemorySearchService(ISearchableParser searchableParser, ISearchableAdapter searchableAdapter)
        {
            Argument.IsNotNull(() => searchableParser);
            Argument.IsNotNull(() => searchableAdapter);

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

        public event EventHandler<EventArgs> Searched;
        #endregion

        #region Methods
        public void AddObjects(IEnumerable<object> searchables)
        {
            Initialize();

            lock (_lockObject)
            {
                Updating.SafeInvoke(this);

                using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
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

                                if (!_searchFields.Contains(searchableProperty.Name))
                                {
                                    _searchFields.Add(searchableProperty.Name);
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

        public void RemoveObjects(IEnumerable<object> searchables)
        {
            Initialize();

            lock (_lockObject)
            {
                Updating.SafeInvoke(this);

                using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
                {
                    using (var writer = new IndexWriter(_indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        foreach (var searchable in searchables)
                        {
                            //writer.DeleteDocuments();
                        }

                        writer.Optimize();
                        writer.Commit();
                    }
                }

                Updated.SafeInvoke(this);
            }
        }

        public IEnumerable<object> Search(string filter, int maxResults = SearchDefaults.DefaultResults)
        {
            Initialize();

            var results = new List<object>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                return results;
            }

            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                var parser = new MultiFieldQueryParser(Version.LUCENE_30, _searchFields.ToArray(), analyzer);
                parser.DefaultOperator = QueryParser.Operator.OR;
                if (!filter.Contains("*") &&
                    !filter.Contains(":") &&
                    !filter.Contains(" ") &&
                    !filter.Contains("AND") &&
                    !filter.Contains("OR"))
                {
                    filter += "*";
                }

                var query = parser.Parse(filter);

                lock (_lockObject)
                {
                    Searching.SafeInvoke(this);

                    var search = _indexSearcher.Search(query, maxResults);
                    foreach (var scoreDoc in search.ScoreDocs)
                    {
                        var score = scoreDoc.Score;
                        var docId = scoreDoc.Doc;
                        var doc = _indexSearcher.Doc(docId);

                        var index = int.Parse(doc.Get(IndexId));
                        results.Add(_indexedObjects[index]);
                    }

                    Searched.SafeInvoke(this);
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

            _indexDirectory = new RAMDirectory();

            // Required to create empty index, which is required for our reader
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var indexWriter = new IndexWriter(_indexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    indexWriter.Commit();
                }
            }

            _indexSearcher = new IndexSearcher(_indexDirectory);
        }
        #endregion
    }
}