namespace Orc.Search
{
    using Metadata;

    public class Searchable : ObjectWithMetadata, ISearchable
    {
        public Searchable(object instance, IMetadataCollection metadataCollection) 
            : base(instance, metadataCollection)
        {
        }
    }
}
