// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemorySearchService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Lucene.Net.Store;

    public class InMemorySearchService : SearchServiceBase
    {
        #region Constructors
        public InMemorySearchService(ISearchQueryService searchQueryService)
            : base(searchQueryService)
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