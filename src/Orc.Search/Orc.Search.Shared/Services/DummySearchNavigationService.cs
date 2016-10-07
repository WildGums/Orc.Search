// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchNavigationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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