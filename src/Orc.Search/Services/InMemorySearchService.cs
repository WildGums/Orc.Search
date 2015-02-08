// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemorySearchService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Lucene.Net.Store;

    public class InMemorySearchService : SearchServiceBase
    {
        #region Constructors
        public InMemorySearchService(ISearchableParser searchableParser, ISearchableAdapter searchableAdapter)
            : base(searchableParser, searchableAdapter)
        {
        }
        #endregion

        #region Methods
        protected override Directory GetDirectory()
        {
            return new RAMDirectory();
        }
        #endregion
    }
}