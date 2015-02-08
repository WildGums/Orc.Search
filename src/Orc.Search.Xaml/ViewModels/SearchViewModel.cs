// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class SearchViewModel : ViewModelBase
    {
        private readonly ISearchService _searchService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ISearchHistoryService _searchHistoryService;

        #region Fields
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

            FilterHistory = new ObservableCollection<string>();

            BuildFilter = new Command(OnBuildFilterExecute);
        }
        #endregion

        #region Properties
        public string Filter { get; set; }

        public ObservableCollection<string> FilterHistory { get; private set; } 
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
        }

        protected override async  Task Close()
        {
            await base.Close();
        }

        private async void OnFilterChanged()
        {
            await _searchService.SearchAsync(Filter);
        }
        #endregion
    }
}