namespace Orc.Search
{
    public interface ISearchableMetadataValue
    {
        ISearchableMetadata Metadata { get; }
        string Value { get; }
    }
}
