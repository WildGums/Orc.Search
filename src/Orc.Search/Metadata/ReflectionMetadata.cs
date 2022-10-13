namespace Orc.Search
{
    using System.Reflection;
    using Metadata;

    public class SearchableMetadata : ReflectionMetadata, ISearchableMetadata
    {
        public SearchableMetadata(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            SearchName = string.Empty;
        }

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
