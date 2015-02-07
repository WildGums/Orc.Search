// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly IDataGenerationService _dataGenerationService;
        private readonly ISearchService _searchService;

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

        public string Filter { get; set; }

        public FastObservableCollection<object> AllObjects { get; private set; }

        public FastObservableCollection<object> FilteredObjects { get; private set; }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

            _searchService.Updating += OnSearchServiceUpdating;
            _searchService.Updated += OnSearchServiceUpdated;

            _searchService.Searching += OnSearchServiceSearching;
            _searchService.Searched += OnSearchServiceSearched;

            using (AllObjects.SuspendChangeNotifications())
            {
                var generatedObjects = await _dataGenerationService.GenerateObjectsAsync();

                AllObjects.ReplaceRange(generatedObjects);

                await _searchService.AddObjectsAsync(generatedObjects);
            }

            UpdateFilter();
        }

        protected override async Task Close()
        {
            _searchService.Updating -= OnSearchServiceUpdating;
            _searchService.Updated -= OnSearchServiceUpdated;

            _searchService.Searching -= OnSearchServiceSearching;
            _searchService.Searched -= OnSearchServiceSearched;

            await base.Close();
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
        }

        private void OnSearchServiceSearched(object sender, EventArgs e)
        {
            IsSearching = false;
        }

        private void OnFilterChanged()
        {
            UpdateFilter();
        }

        private async void UpdateFilter()
        {
            using (FilteredObjects.SuspendChangeNotifications())
            {
                var searchResults = await _searchService.SearchAsync(Filter);

                FilteredObjects.ReplaceRange(searchResults);
            }
        }
        #endregion
    }
}