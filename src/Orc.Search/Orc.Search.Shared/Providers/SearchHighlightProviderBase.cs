// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHighlightProviderBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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