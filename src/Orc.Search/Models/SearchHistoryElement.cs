// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHistoryElement.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Catel.Data;

    public class SearchHistoryElement : ModelBase
    {
        #region Properties
        public string Filter { get; set; }
        public bool EverFoundResults { get; set; }
        public int Count { get; set; }
        #endregion
    }
}