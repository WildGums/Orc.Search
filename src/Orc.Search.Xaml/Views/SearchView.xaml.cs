namespace Orc.Search
{
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM.Views;
    using Microsoft.Extensions.DependencyInjection;

    public partial class SearchView
    {
        [ViewToViewModel]
        public string? Filter
        {
            get { return (string?)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string),
            typeof(SearchView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public int MaxResultsCount
        {
            get { return (int)GetValue(MaxResultsCountProperty); }
            set { SetValue(MaxResultsCountProperty, value); }
        }

        public static readonly DependencyProperty MaxResultsCountProperty = DependencyProperty.Register(nameof(MaxResultsCount), typeof(int),
            typeof(SearchView), new FrameworkPropertyMetadata(SearchDefaults.DefaultResults));
    }
}
