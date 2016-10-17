// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;
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

        private void OnBuildFilterExecute()
        {
            var vm = _viewModelFactory.CreateViewModel<SearchFilterBuilderViewModel>(null, null);
            if (_uiVisualizerService.ShowDialog(vm) ?? false)
            {
                Filter = vm.Filter;
            }
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _dispatcherTimer.Tick += OnDispatcherTimerTick;
        }

        protected override async Task CloseAsync()
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= OnDispatcherTimerTick;

            await base.CloseAsync();
        }

        private void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();

#pragma warning disable 4014
            SearchAsync();
#pragma warning restore 4014
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
                ((ICollection<string>)FilterHistory).ReplaceRange(_searchHistoryService.GetLastSearchQueries(filter));
            }
        }

        private Task SearchAsync()
        {
            return Task.Factory.StartNew(() => _searchService.Search(Filter));
        }
        #endregion
    }
}