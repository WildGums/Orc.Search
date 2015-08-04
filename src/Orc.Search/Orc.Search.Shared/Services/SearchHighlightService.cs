// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHighlightService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.Logging;
    using Catel.Threading;

    public class SearchHighlightService : ISearchHighlightService
    {
        private readonly ISearchService _searchService;

        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly List<ISearchHighlightProvider> _providers = new List<ISearchHighlightProvider>();
        #endregion

        #region Constructors
        public SearchHighlightService(ISearchService searchService)
        {
            Argument.IsNotNull(() => searchService);

            _searchService = searchService;
            _searchService.Searched += OnSearched;
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs> Highlighting;
        public event EventHandler<EventArgs> Highlighted;
        #endregion

        #region Methods
        public void AddProvider(ISearchHighlightProvider provider)
        {
            Argument.IsNotNull(() => provider);

            lock (_providers)
            {
                _providers.Add(provider);
            }
        }

        public void RemoveProvider(ISearchHighlightProvider provider)
        {
            Argument.IsNotNull(() => provider);

            lock (_providers)
            {
                _providers.Remove(provider);
            }
        }

        public void ResetHighlights()
        {
            lock (_providers)
            {
                Highlighting.SafeInvoke(this);

                foreach (var provider in _providers)
                {
                    provider.ResetHighlight();
                }

                Highlighted.SafeInvoke(this);
            }
        }

        public void HighlightSearchables(IEnumerable<object> searchables)
        {
            lock (_providers)
            {
                Highlighting.SafeInvoke(this);

                foreach (var searchable in searchables)
                {
                    foreach (var provider in _providers)
                    {
                        provider.HighlightSearchable(searchable);
                    }
                }

                Highlighted.SafeInvoke(this);
            }
        }

        private async void OnSearched(object sender, SearchEventArgs e)
        {
            await TaskHelper.Run(() => HighlightSearchables(e.Results));
        }
        #endregion
    } 
}