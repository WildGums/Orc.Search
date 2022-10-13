namespace Orc.Search
{
    public class ReflectionSearchable : Searchable
    {
        public ReflectionSearchable(object instance) 
            : base(instance, new AttributeMetadataCollection(instance.GetType()))
        {
        }
    }
}
