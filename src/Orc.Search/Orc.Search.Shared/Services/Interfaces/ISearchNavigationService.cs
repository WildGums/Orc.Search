// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchNavigationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public interface ISearchNavigationService
    {
        #region Methods
        void Navigate(object searchable);
        #endregion
    }
}