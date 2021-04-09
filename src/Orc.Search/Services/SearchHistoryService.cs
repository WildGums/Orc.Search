// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHistoryService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
    using Catel.Runtime.Serialization.Xml;
    using Catel.Services;
    using Catel.Threading;
    using Orc.FileSystem;

    public class SearchHistoryService : ISearchHistoryService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IXmlSerializer _xmlSerializer;
        private readonly IAppDataService _appDataService;
        private readonly IDirectoryService _directoryService;
        private readonly object _lock = new object();
        private readonly string _fileName;
        private readonly SearchHistory _searchHistory = new SearchHistory();
        #endregion

        #region Constructors
        public SearchHistoryService(ISearchService searchService, IXmlSerializer xmlSerializer,
            IAppDataService appDataService, IDirectoryService directoryService)
        {
            Argument.IsNotNull(() => searchService);
            Argument.IsNotNull(() => xmlSerializer);
            Argument.IsNotNull(() => appDataService);
            Argument.IsNotNull(() => directoryService);

            _xmlSerializer = xmlSerializer;
            _appDataService = appDataService;
            _directoryService = directoryService;
            searchService.Searched += OnSearchServiceSearched;

            var directory = Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "search");
            _directoryService.Create(directory);

            _fileName = Path.Combine(directory, "history.xml");

            LoadSearchHistory();
        }
        #endregion

        #region Properties
        #endregion

        #region Methods
        public IEnumerable<string> GetLastSearchQueries(string prefix, int count = 5)
        {
            var elements = new List<string>();

            lock (_lock)
            {
                prefix = prefix.ToLower();

                elements.AddRange((from element in _searchHistory.SearchHistoryElements
                                   where element.FilterLowerCase.StartsWith(prefix)
                                   orderby element.Count
                                   select element.Filter).Take(count));
            }

            return elements;
        }

        private void OnSearchServiceSearched(object sender, SearchEventArgs e)
        {
#pragma warning disable 4014
            TaskHelper.Run(() => AddSearchFilterToHistory(e.Filter, e.Results), true);
#pragma warning restore 4014
        }

        private void AddSearchFilterToHistory(string filter, IEnumerable<object> results)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return;
            }

            filter = filter.Trim();

            lock (_lock)
            {
                SearchHistoryElement searchHistoryElement = null;

                foreach (var searchHistory in _searchHistory.SearchHistoryElements)
                {
                    if (string.Equals(searchHistory.Filter, filter))
                    {
                        searchHistoryElement = searchHistory;
                        break;
                    }
                }

                if (searchHistoryElement is null)
                {
                    searchHistoryElement = new SearchHistoryElement();
                    searchHistoryElement.Filter = filter;

                    _searchHistory.SearchHistoryElements.Add(searchHistoryElement);
                }

                searchHistoryElement.Count++;

                if (!searchHistoryElement.EverFoundResults && results.Any())
                {
                    searchHistoryElement.EverFoundResults = true;
                }

                SaveSearchHistory();
            }
        }

        private void LoadSearchHistory()
        {
            try
            {
                lock (_lock)
                {
                    Log.Debug("Loading search history");

                    if (!File.Exists(_fileName))
                    {
                        Log.Debug("History file does not exist, skipping loading");
                        return;
                    }

                    using (var fileStream = new FileStream(_fileName, FileMode.OpenOrCreate))
                    {
                        _xmlSerializer.Deserialize(_searchHistory, fileStream, null);
                    }

                    Log.Debug("Loaded search history");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load search history");
            }
        }

        private void SaveSearchHistory()
        {
            try
            {
                lock (_lock)
                {
                    Log.Debug("Saving search history");

                    using (var fileStream = new FileStream(_fileName, FileMode.Create))
                    {
                        _xmlSerializer.Serialize(_searchHistory, fileStream, null);
                    }

                    Log.Debug("Saved search history");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save search history");
            }
        }
        #endregion
    }
}
