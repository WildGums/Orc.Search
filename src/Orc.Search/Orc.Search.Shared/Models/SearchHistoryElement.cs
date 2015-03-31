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
        private string _filterLowerCase;

        #region Properties
        public string Filter { get; set; }

        public string FilterLowerCase
        {
            get { return _filterLowerCase; }
        }

        public bool EverFoundResults { get; set; }
        public int Count { get; set; }

        private void OnFilterChanged()
        {
            var filter = Filter;

            if (string.IsNullOrWhiteSpace(filter))
            {
                _filterLowerCase = filter;
            }
            else
            {
                _filterLowerCase = filter.ToLower();
            }
        }

        protected override void OnDeserialized()
        {
            base.OnDeserialized();

            OnFilterChanged();
        }
        #endregion
    }
}