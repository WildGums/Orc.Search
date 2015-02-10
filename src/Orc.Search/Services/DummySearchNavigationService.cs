// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchNavigationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public class DummySearchNavigationService : ISearchNavigationService
    {
        #region Methods
        public void Navigate(object searchable)
        {
            // by default, this implementation has no navigation
        }
        #endregion
    }
}