// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchHighlightService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;

    public interface ISearchHighlightService
    {
        event EventHandler<EventArgs> Highlighting;
        event EventHandler<EventArgs> Highlighted;

        #region Methods
        void AddProvider(ISearchHighlightProvider provider);
        void RemoveProvider(ISearchHighlightProvider provider);

        void ResetHighlights();
        void HighlightSearchables(IEnumerable<object> searchables);
        #endregion
    }
}