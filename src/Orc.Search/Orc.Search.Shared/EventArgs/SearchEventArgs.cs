// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchEventArgs.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;

    public class SearchEventArgs : EventArgs
    {
        public SearchEventArgs(string filter, IEnumerable<object> results)
        {
            Filter = filter;
            Results = results;
        }

        public string Filter { get; private set; }

        public IEnumerable<object> Results { get; private set; }
    }
}