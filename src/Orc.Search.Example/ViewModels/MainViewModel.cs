// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly IDataGenerationService _dataGenerationService;
        private readonly ISearchService _searchService;
        private readonly IUIVisualizerService _uiVisualizerService;

        private readonly Stopwatch _searchStopwatch = new Stopwatch();

        #region Constructors
        public MainViewModel(IDataGenerationService dataGenerationService, ISearchService searchService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => dataGenerationService);
            Argument.IsNotNull(() => searchService);
            Argument.IsNotNull(() => uiVisualizerService);

            _dataGenerationService = dataGenerationService;
            _searchService = searchService;
            _uiVisualizerService = uiVisualizerService;

            AllObjects = new FastObservableCollection<object>();
            FilteredObjects = new FastObservableCollection<object>();

            AddPersion = new Command(OnAddPerson);
        }

        private void OnAddPerson()
        {
            var addPersonViewModel = new AddPersonViewModel();
            if (_uiVisualizerService.ShowDialog(addPersonViewModel) ?? false)
            {
                var person = addPersonViewModel.Person;
                var searchable = _dataGenerationService.GenerateSearchable(person);

                _searchService.AddObjects(new[] { searchable });
                AllObjects.Add(searchable);
            }  
        }
        #endregion

        public Command AddPersion { get; private set; }

        #region Properties
        public override string Title
        {
            get { return "Orc.Search example"; }
        }

        public int IndexedObjectCount { get; private set; }

        public bool IsUpdatingSearch { get; private set; }

        public bool IsSearching { get; private set; }

        public FastObservableCollection<object> AllObjects { get; private set; }

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

                ((ICollection<object>)AllObjects).ReplaceRange(generatedSearchables);
                AllObjects.CollectionChanged += AllObjectsOnCollectionChanged;

                await TaskHelper.Run(() => _searchService.AddObjects(generatedSearchables), true);
            }
        }

        private void AllObjectsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                var oldItems = args.OldItems;
                _searchService.RemoveObjects(oldItems.OfType<ISearchable>());
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
                ((ICollection<object>)filteredObjects).ReplaceRange(e.Results);
                FilteredObjectCount = filteredObjects.Count;
            }

            IsSearching = false;
        }
        #endregion
    }
}