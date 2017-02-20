// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
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
        #region Fields
        private readonly IDataGenerationService _dataGenerationService;
        private readonly ISearchService _searchService;

        private readonly Stopwatch _searchStopwatch = new Stopwatch();
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

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

            AddPerson = new Command(OnAddPerson);
            RemovePerson = new Command(OnRemovePerson);
        }

        #endregion

        #region Properties
        public Command AddPerson { get; private set; }

        public Command RemovePerson { get; private set; }

        public override string Title
        {
            get { return "Orc.Search example"; }
        }

        public object SelectedObject { get; set; }

        public int IndexedObjectCount { get; private set; }

        public bool IsUpdatingSearch { get; private set; }

        public bool IsSearching { get; private set; }

        public string Filter { get; set; }

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
                AllObjects.CollectionChanged += OnAllObjectsOnCollectionChanged;

                await TaskHelper.Run(() => _searchService.AddObjects(generatedSearchables), true);
            }
        }

        private void OnAddPerson()
        {
            var addPersonViewModel = new AddPersonViewModel();
            if (_uiVisualizerService.ShowDialog(addPersonViewModel) ?? false)
            {
                var person = addPersonViewModel.Person;
                var searchable = _dataGenerationService.GenerateSearchable(person);

                _searchService.AddObjects(new[] {searchable});
                AllObjects.Add(searchable);

                _searchService.Search(Filter);
            }
        }
        private void OnRemovePerson()
        {
            var selectedSearchable = SelectedObject as ISearchable;
            if (selectedSearchable == null)
            {
                return;
            }

            AllObjects.Remove(selectedSearchable);
        }

        private void OnAllObjectsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                var oldItems = args.OldItems;
                _searchService.RemoveObjects(oldItems.OfType<ISearchable>());
            }

            _searchService.Search(Filter);
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
                ((ICollection<object>) filteredObjects).ReplaceRange(e.Results);
                FilteredObjectCount = filteredObjects.Count;
            }

            IsSearching = false;
        }
        #endregion
    }
}