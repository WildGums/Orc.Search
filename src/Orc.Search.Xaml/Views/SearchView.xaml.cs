// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Data;
    using System.Windows;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView
    {
        #region Constructors
        static SearchView()
        {
            typeof(SearchView).AutoDetectViewPropertiesToSubscribe();
        }

        public SearchView()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties

        [ViewToViewModel]
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), 
            typeof(SearchView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType=ViewToViewModelMappingType.ViewToViewModel)]
        public int MaxResultsCount
        {
            get { return (int)GetValue(MaxResultsCountProperty); }
            set { SetValue(MaxResultsCountProperty, value); }
        }

        public static readonly DependencyProperty MaxResultsCountProperty = DependencyProperty.Register(nameof(MaxResultsCount), typeof(int),
            typeof(SearchView), new FrameworkPropertyMetadata(SearchDefaults.DefaultResults));
        #endregion
    }
}
