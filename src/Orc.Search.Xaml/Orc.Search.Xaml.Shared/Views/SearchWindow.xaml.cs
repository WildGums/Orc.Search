// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for SearchWindow.xaml.
    /// </summary>
    public partial class SearchWindow : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchWindow"/> class.
        /// </summary>
        public SearchWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SearchWindow(SearchViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
        #endregion
    }
}