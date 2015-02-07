// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchableAdapter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public interface ISearchableAdapter
    {
        #region Methods
        object GetValue(object searchable, SearchableProperty searchableProperty);
        #endregion
    }
}