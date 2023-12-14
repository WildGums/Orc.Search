namespace Orc.Search
{
    public partial class SearchFilterBuilderWindow
    {
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
        public SearchFilterBuilderWindow(SearchFilterBuilderViewModel? viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
