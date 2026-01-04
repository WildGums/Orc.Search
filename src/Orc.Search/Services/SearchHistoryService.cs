namespace Orc.Search
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.Services;
    using Microsoft.Extensions.Logging;
    using Orc.FileSystem;
    using Orc.Serialization.Json;

    public class SearchHistoryService : ISearchHistoryService
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(SearchHistoryService));

        private readonly IJsonSerializerFactory _jsonSerializerFactory;
        private readonly IAppDataService _appDataService;
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly object _lock = new object();
        private readonly string _fileName;

        private SearchHistory _searchHistory = new SearchHistory();

        public SearchHistoryService(ISearchService searchService, IJsonSerializerFactory jsonSerializerFactory,
            IAppDataService appDataService, IDirectoryService directoryService, IFileService fileService)
        {
            _jsonSerializerFactory = jsonSerializerFactory;
            _appDataService = appDataService;
            _directoryService = directoryService;
            _fileService = fileService;

            searchService.Searched += OnSearchServiceSearched;

            var directory = Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "search");
            _directoryService.Create(directory);

            _fileName = Path.Combine(directory, "history.json");

            LoadSearchHistory();
        }

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

        private void OnSearchServiceSearched(object? sender, SearchEventArgs e)
        {
#pragma warning disable 4014
            Task.Run(() => AddSearchFilterToHistory(e.Filter, e.Results));
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
                SearchHistoryElement? searchHistoryElement = null;

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
                    Logger.LogDebug("Loading search history");

                    if (!_fileService.Exists(_fileName))
                    {
                        Logger.LogDebug("History file does not exist, skipping loading");
                        return;
                    }

                    var serializer = _jsonSerializerFactory.CreateSerializer();

                    using (var fileStream = _fileService.OpenRead(_fileName))
                    {
                        _searchHistory = serializer.Deserialize<SearchHistory>(fileStream) ?? new SearchHistory();
                    }

                    Logger.LogDebug("Loaded search history");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load search history");
            }
        }

        private void SaveSearchHistory()
        {
            try
            {
                lock (_lock)
                {
                    Logger.LogDebug("Saving search history");

                    var serializer = _jsonSerializerFactory.CreateSerializer();

                    using (var fileStream = _fileService.Create(_fileName))
                    {
                        serializer.Serialize(fileStream, _searchHistory);
                    }

                    Logger.LogDebug("Saved search history");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to save search history");
            }
        }
    }
}
