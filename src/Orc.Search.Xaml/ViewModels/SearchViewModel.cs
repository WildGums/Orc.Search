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
        private readonly ISearchService _searchService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISearchHistoryService _searchHistoryService;

        private readonly DispatcherTimer _dispatcherTimer;

        public SearchViewModel(ISearchService searchService, IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory,
            ISearchHistoryService searchHistoryService)
        {
            ArgumentNullException.ThrowIfNull(searchService);
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(viewModelFactory);
            ArgumentNullException.ThrowIfNull(searchHistoryService);

            _searchService = searchService;
            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
            _searchHistoryService = searchHistoryService;

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);

            FilterHistory = new FastObservableCollection<string>();

            BuildFilter = new TaskCommand(OnBuildFilterExecuteAsync);
        }

        public string? Filter { get; set; }

        public int MaxResultsCount { get; set; }

        public FastObservableCollection<string> FilterHistory { get; private set; }

        public TaskCommand BuildFilter { get; private set; }

        private async Task OnBuildFilterExecuteAsync()
        {
            var vm = _viewModelFactory.CreateRequiredViewModel<SearchFilterBuilderViewModel>(null, null);
            var result = await _uiVisualizerService.ShowDialogAsync(vm);
            if (result.DialogResult ?? false)
            {
                Filter = vm.Filter;
            }
        }

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

#pragma warning disable AvoidAsyncVoid
        private async void OnDispatcherTimerTick(object? sender, EventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
            _dispatcherTimer.Stop();

            await SearchAsync();
        }

        private void OnFilterChanged()
        {
            var filter = Filter;
            if (string.IsNullOrWhiteSpace(filter))
            {
                return;
            }

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

        private Task SearchAsync()
        {
            return Task.Run(() =>
            {
                var results = _searchService.Search(Filter ?? string.Empty);
            });
        }
    }
}
