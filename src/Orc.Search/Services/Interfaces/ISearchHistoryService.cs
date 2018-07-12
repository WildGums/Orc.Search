// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchHistoryService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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