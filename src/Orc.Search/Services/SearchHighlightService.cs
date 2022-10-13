namespace Orc.Search
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Threading;

    public class SearchHighlightService : ISearchHighlightService
    {
        private readonly List<ISearchHighlightProvider> _providers = new List<ISearchHighlightProvider>();

        public SearchHighlightService(ISearchService searchService)
        {
            ArgumentNullException.ThrowIfNull(searchService);

            searchService.Searched += OnSearched;
        }

        public event EventHandler<EventArgs>? Highlighting;
        public event EventHandler<EventArgs>? Highlighted;

        public void AddProvider(ISearchHighlightProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);

            lock (_providers)
            {
                _providers.Add(provider);
            }
        }

        public void RemoveProvider(ISearchHighlightProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);

            lock (_providers)
            {
                _providers.Remove(provider);
            }
        }

        public void ResetHighlights()
        {
            lock (_providers)
            {
                Highlighting?.Invoke(this, EventArgs.Empty);

                foreach (var provider in _providers)
                {
                    provider.ResetHighlight();
                }

                Highlighted?.Invoke(this, EventArgs.Empty);
            }
        }

        public void HighlightSearchables(IEnumerable<object> searchables)
        {
            lock (_providers)
            {
                Highlighting?.Invoke(this, EventArgs.Empty);

                foreach (var searchable in searchables)
                {
                    foreach (var provider in _providers)
                    {
                        provider.HighlightSearchable(searchable);
                    }
                }

                Highlighted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnSearched(object? sender, SearchEventArgs e)
        {
#pragma warning disable 4014
            Task.Run(() => HighlightSearchables(e.Results));
#pragma warning restore 4014
        }
    } 
}
