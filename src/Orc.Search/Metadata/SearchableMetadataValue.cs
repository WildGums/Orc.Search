namespace Orc.Search
{
    using System;

    public class SearchableMetadataValue : ISearchableMetadataValue
    {
        public SearchableMetadataValue(ISearchableMetadata metadata, string value)
        {
            ArgumentNullException.ThrowIfNull(metadata);
            ArgumentNullException.ThrowIfNull(value);

            Metadata = metadata;
            Value = value;
        }

        public ISearchableMetadata Metadata { get; private set; }

        public string Value { get; private set; }
    }
}
