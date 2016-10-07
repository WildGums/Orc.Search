// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchHighlightProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public interface ISearchHighlightProvider
    {
        void ResetHighlight();

        void HighlightSearchable(object searchable);
    }
}