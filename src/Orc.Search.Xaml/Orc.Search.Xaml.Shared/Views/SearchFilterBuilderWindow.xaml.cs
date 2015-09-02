// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchFilterBuilderWindow.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    /// <summary>
    /// Interaction logic for SearchFilterBuilderWindow.xaml.
    /// </summary>
    public partial class SearchFilterBuilderWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFilterBuilderWindow"/> class.
        /// </summary>
        public SearchFilterBuilderWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFilterBuilderWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SearchFilterBuilderWindow(SearchFilterBuilderViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
        #endregion
    }
}