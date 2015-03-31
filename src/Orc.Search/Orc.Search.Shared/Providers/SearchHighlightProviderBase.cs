// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHighlightProviderBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public abstract class SearchHighlightProviderBase : ISearchHighlightProvider
    {
        public virtual void ResetHighlight()
        {
            
        }

        public virtual void HighlightSearchable(object searchable)
        {
            
        }
    }
}