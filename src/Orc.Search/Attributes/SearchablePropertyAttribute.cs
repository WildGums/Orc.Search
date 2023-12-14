namespace Orc.Search
{
    using System;

    /// <summary>
    /// Defines a property as searchable so it will be indexed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchablePropertyAttribute : Attribute
    {
        public SearchablePropertyAttribute()
        {
            SearchName = string.Empty;
            Analyze = true;
        }

        /// <summary>
        /// Gets or sets the search name. If not specified, this will equal the property name.
        /// </summary>
        /// <value>The name.</value>
        public string SearchName { get; set; }

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
