// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchHistoryService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Collections.Generic;

    public interface ISearchHistoryService
    {
        #region Methods
        IEnumerable<string> GetLastSearchQueries(string prefix, int count = 5);
        #endregion
    }
}