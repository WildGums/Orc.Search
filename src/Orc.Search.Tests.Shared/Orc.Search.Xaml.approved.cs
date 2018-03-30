[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Search
{
    public class SearchFilterBuilderViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData FilterProperty;
        public SearchFilterBuilderViewModel() { }
        public string Filter { get; set; }
    }
    public class SearchFilterBuilderWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public SearchFilterBuilderWindow() { }
        public SearchFilterBuilderWindow(Orc.Search.SearchFilterBuilderViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class SearchHighlight : Catel.Windows.Interactivity.BehaviorBase<System.Windows.FrameworkElement>, Orc.Search.ISearchHighlightProvider
    {
        public static readonly System.Windows.DependencyProperty HighlightStyleProperty;
        public static readonly System.Windows.DependencyProperty SearchableProperty;
        public static readonly System.Windows.DependencyProperty StylePropertyNameProperty;
        public SearchHighlight() { }
        public System.Windows.Style HighlightStyle { get; set; }
        public object Searchable { get; set; }
        public string StylePropertyName { get; set; }
        public void HighlightSearchable(object searchable) { }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
        public void ResetHighlight() { }
    }
    public class SearchView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty FilterProperty;
        public static readonly System.Windows.DependencyProperty MaxResultsCountProperty;
        public SearchView() { }
        [Catel.MVVM.Views.ViewToViewModelAttribute("")]
        public string Filter { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewToViewModel)]
        public int MaxResultsCount { get; set; }
        public void InitializeComponent() { }
    }
    public class SearchViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData FilterHistoryProperty;
        public static readonly Catel.Data.PropertyData FilterProperty;
        public static readonly Catel.Data.PropertyData MaxResultsCountProperty;
        public SearchViewModel(Orc.Search.ISearchService searchService, Catel.Services.IUIVisualizerService uiVisualizerService, Catel.MVVM.IViewModelFactory viewModelFactory, Orc.Search.ISearchHistoryService searchHistoryService) { }
        public Catel.MVVM.TaskCommand BuildFilter { get; }
        public string Filter { get; set; }
        public Catel.Collections.FastObservableCollection<string> FilterHistory { get; }
        public int MaxResultsCount { get; set; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public class SearchWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public SearchWindow() { }
        public SearchWindow(Orc.Search.SearchViewModel viewModel) { }
        public void InitializeComponent() { }
    }
}