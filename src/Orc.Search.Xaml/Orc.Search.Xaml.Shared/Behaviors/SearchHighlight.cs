// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchableBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Windows;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Windows.Data;
    using Catel.Windows.Interactivity;

    public class SearchHighlight : BehaviorBase<FrameworkElement>, ISearchHighlightProvider
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly ISearchHighlightService _searchHighlightService;

        private Style _defaultStyle;
        private bool _isHighlighted;
        private bool _wasHighlightedBetweenEvents;
        private object _searchable;
        private DependencyProperty _styleDependencyProperty;

        public SearchHighlight()
        {
            var dependencyResolver = this.GetDependencyResolver();

            _dispatcherService = dependencyResolver.Resolve<IDispatcherService>();
            _searchHighlightService = dependencyResolver.Resolve<ISearchHighlightService>();

            _searchHighlightService.Highlighting += OnSearchHighlightServiceHighlighting;
            _searchHighlightService.Highlighted += OnSearchHighlightServiceHighlighted;
        }

        #region Properties
        public object Searchable
        {
            get { return (object)GetValue(SearchableProperty); }
            set { SetValue(SearchableProperty, value); }
        }

        public static readonly DependencyProperty SearchableProperty = DependencyProperty.Register("Searchable", typeof(object),
            typeof(SearchHighlight), new PropertyMetadata(null, (sender, e) => ((SearchHighlight)sender).OnSearchableChanged()));

        public string StylePropertyName
        {
            get { return (string)GetValue(StylePropertyNameProperty); }
            set { SetValue(StylePropertyNameProperty, value); }
        }

        public static readonly DependencyProperty StylePropertyNameProperty = DependencyProperty.Register("StylePropertyName", typeof(string),
            typeof(SearchHighlight), new PropertyMetadata("Style", (sender, e) => ((SearchHighlight)sender).GetStyleDependencyProperty()));

        public Style HighlightStyle
        {
            get { return (Style)GetValue(HighlightStyleProperty); }
            set { SetValue(HighlightStyleProperty, value); }
        }

        public static readonly DependencyProperty HighlightStyleProperty =
            DependencyProperty.Register("HighlightStyle", typeof(Style), typeof(SearchHighlight), new PropertyMetadata(null));
        #endregion

        #region Methods
        public void ResetHighlight()
        {
            if (!_isHighlighted)
            {
                return;
            }

            _dispatcherService.BeginInvokeIfRequired(() =>
            {
                _isHighlighted = false;

                AssociatedObject.SetValue(_styleDependencyProperty, _defaultStyle);
                _defaultStyle = null;
            });
        }

        public void HighlightSearchable(object searchable)
        {
            if (!ReferenceEquals(searchable, _searchable))
            {
                return;
            }

            _wasHighlightedBetweenEvents = true;

            _dispatcherService.BeginInvokeIfRequired(() =>
            {
                if (!_isHighlighted)
                {
                    _defaultStyle = AssociatedObject.GetValue(_styleDependencyProperty) as Style;
                }

                _isHighlighted = true;

                AssociatedObject.SetValue(_styleDependencyProperty, HighlightStyle);
            });
        }

        private void OnSearchHighlightServiceHighlighting(object sender, EventArgs e)
        {
            _wasHighlightedBetweenEvents = false;
        }

        private void OnSearchHighlightServiceHighlighted(object sender, EventArgs e)
        {
            if (!_wasHighlightedBetweenEvents)
            {
                ResetHighlight();
            }
        }

        private void OnSearchableChanged()
        {
            _searchable = Searchable;
        }

        private void GetStyleDependencyProperty()
        {
            _styleDependencyProperty = AssociatedObject.GetDependencyPropertyByName(StylePropertyName);
        }

        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            GetStyleDependencyProperty();

            _searchHighlightService.AddProvider(this);
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            _searchHighlightService.RemoveProvider(this);

            _searchable = null;

            base.OnAssociatedObjectUnloaded();
        }
        #endregion
    }
}