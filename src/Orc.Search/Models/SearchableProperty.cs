// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchableProperty.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public class SearchableProperty
    {
        public SearchableProperty(string propertyName)
        {
            Name = propertyName;
            PropertyName = propertyName;
        }

        public string Name { get; set; }

        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property must be analyzed. Set this value to <c>true</c> for common text 
        /// or <c>false</c> for unique keys such as product ids.
        /// <para />
        /// The default value is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if analyze; otherwise, <c>false</c>.</value>
        public bool Analyze { get; set; }
    }
}