namespace Orc.Search
{
    using System;
    using System.Collections.Generic;

    public interface ISearchHighlightService
    {
        event EventHandler<EventArgs>? Highlighting;
        event EventHandler<EventArgs>? Highlighted;

        void AddProvider(ISearchHighlightProvider provider);
        void RemoveProvider(ISearchHighlightProvider provider);

        void ResetHighlights();
        void HighlightSearchables(IEnumerable<object> searchables);
    }
}
