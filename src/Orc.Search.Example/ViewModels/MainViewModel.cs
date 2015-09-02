// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Threading;
    using Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly IDataGenerationService _dataGenerationService;
        private readonly ISearchService _searchService;

        private readonly Stopwatch _searchStopwatch = new Stopwatch();

        #region Constructors
        public MainViewModel(IDataGenerationService dataGenerationService, ISearchService searchService)
        {
            Argument.IsNotNull(() => dataGenerationService);
            Argument.IsNotNull(() => searchService);

            _dataGenerationService = dataGenerationService;
            _searchService = searchService;

            AllObjects = new FastObservableCollection<object>();
            FilteredObjects = new FastObservableCollection<object>();
        }
        #endregion

        #region Properties
        public override string Title
        {
            get { return "Orc.Search example"; }
        }

        public int IndexedObjectCount { get; private set; }

        public bool IsUpdatingSearch { get; private set; }

        public bool IsSearching { get; private set; }

        public FastObservableCollection<object> AllObjects { get; private set; }

        public int AllObjectCount { get; private set; }

        public FastObservableCollection<object> FilteredObjects { get; private set; }

        public int FilteredObjectCount { get; private set; }

        public TimeSpan LastSearchDuration { get; private set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _searchService.Updating += OnSearchServiceUpdating;
            _searchService.Updated += OnSearchServiceUpdated;

            _searchService.Searching += OnSearchServiceSearching;
            _searchService.Searched += OnSearchServiceSearched;

            using (AllObjects.SuspendChangeNotifications())
            {
                var generatedSearchables = (await TaskHelper.Run(() => _dataGenerationService.GenerateSearchables(), true)).ToList();

                AllObjects.ReplaceRange(generatedSearchables.Select(x => x.Instance));
                AllObjectCount = AllObjects.Count;

                await TaskHelper.Run(() => _searchService.AddObjects(generatedSearchables), true);
            }
        }

        protected override async Task CloseAsync()
        {
            _searchService.Updating -= OnSearchServiceUpdating;
            _searchService.Updated -= OnSearchServiceUpdated;

            _searchService.Searching -= OnSearchServiceSearching;
            _searchService.Searched -= OnSearchServiceSearched;

            await base.CloseAsync();
        }

        private void OnSearchServiceUpdating(object sender, EventArgs e)
        {
            IsUpdatingSearch = true;
        }

        private void OnSearchServiceUpdated(object sender, EventArgs e)
        {
            IsUpdatingSearch = false;

            IndexedObjectCount = _searchService.IndexedObjectCount;
        }

        private void OnSearchServiceSearching(object sender, EventArgs e)
        {
            IsSearching = true;

            _searchStopwatch.Restart();
        }

        private void OnSearchServiceSearched(object sender, SearchEventArgs e)
        {
            _searchStopwatch.Stop();
            LastSearchDuration = _searchStopwatch.Elapsed;

            var filteredObjects = FilteredObjects;

            using (filteredObjects.SuspendChangeNotifications())
            {
                filteredObjects.ReplaceRange(e.Results.Select(x => x.Instance));
                FilteredObjectCount = filteredObjects.Count;
            }

            IsSearching = false;
        }
        #endregion
    }
}