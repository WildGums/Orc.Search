// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchableBehavior.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Windows;
    using Catel.IoC;
    using Catel.Windows.Interactivity;

    public class SearchHighlight : BehaviorBase<FrameworkElement>
    {
        private readonly ISearchService _searchService;

        public SearchHighlight()
        {
            var dependencyResolver = this.GetDependencyResolver();
            _searchService = dependencyResolver.Resolve<ISearchService>();
        }

        #region Properties
        public Style HitStyle
        {
            get { return (Style)GetValue(HitStyleProperty); }
            set { SetValue(HitStyleProperty, value); }
        }

        public static readonly DependencyProperty HitStyleProperty = 
            DependencyProperty.Register("HitStyle", typeof(Style), typeof(SearchHighlight), new PropertyMetadata(null));
        #endregion

        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            // TODO: unsubscribe

            base.OnAssociatedObjectUnloaded();
        }
    }
}