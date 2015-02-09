// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Services;

    public class SearchViewModel : ViewModelBase
    {
        #region Fields
        private readonly ISearchService _searchService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISearchHistoryService _searchHistoryService;

        private readonly DispatcherTimer _dispatcherTimer;
        #endregion

        #region Constructors
        public SearchViewModel(ISearchService searchService, IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory,
            ISearchHistoryService searchHistoryService)
        {
            Argument.IsNotNull(() => searchService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => viewModelFactory);
            Argument.IsNotNull(() => searchHistoryService);

            _searchService = searchService;
            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
            _searchHistoryService = searchHistoryService;

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);

            FilterHistory = new FastObservableCollection<string>();

            BuildFilter = new Command(OnBuildFilterExecute);
        }
        #endregion

        #region Properties
        public string Filter { get; set; }

        public FastObservableCollection<string> FilterHistory { get; private set; }
        #endregion

        #region Commands
        public Command BuildFilter { get; private set; }

        private async void OnBuildFilterExecute()
        {
            var vm = _viewModelFactory.CreateViewModel<SearchFilterBuilderViewModel>(null);
            if (await _uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Filter = vm.Filter;
            }
        }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

            _dispatcherTimer.Tick += OnDispatcherTimerTick;
        }

        protected override async Task Close()
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= OnDispatcherTimerTick;

            await base.Close();
        }

        private async void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();

            await Search();
        }

        private void OnFilterChanged()
        {
            var filter = Filter;

            if (!IsClosed)
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer.Start();
            }

            using (FilterHistory.SuspendChangeNotifications())
            {
                FilterHistory.ReplaceRange(_searchHistoryService.GetLastSearchQueries(filter));
            }
        }

        private async Task Search()
        {
            await _searchService.SearchAsync(Filter);
        }
        #endregion
    }
}