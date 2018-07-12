﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System;
    using System.Collections.Generic;

    public class SearchEventArgs : EventArgs
    {
        public SearchEventArgs(string filter, IEnumerable<ISearchable> results)
        {
            Filter = filter;
            Results = results;
        }

        public string Filter { get; private set; }

        public IEnumerable<ISearchable> Results { get; private set; }
    }
}