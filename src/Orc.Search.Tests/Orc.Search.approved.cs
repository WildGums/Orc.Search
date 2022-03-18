[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName="")]
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Search
{
    public class AttributeMetadataCollection : Orc.Metadata.ReflectionMetadataCollection
    {
        public AttributeMetadataCollection(System.Type targetType) { }
        public override System.Collections.Generic.IEnumerable<Orc.Metadata.IMetadata> All { get; }
    }
    public class DummySearchNavigationService : Orc.Search.ISearchNavigationService
    {
        public DummySearchNavigationService() { }
        public void Navigate(object searchable) { }
    }
    public interface ISearchHighlightProvider
    {
        void HighlightSearchable(object searchable);
        void ResetHighlight();
    }
    public interface ISearchHighlightService
    {
        event System.EventHandler<System.EventArgs> Highlighted;
        event System.EventHandler<System.EventArgs> Highlighting;
        void AddProvider(Orc.Search.ISearchHighlightProvider provider);
        void HighlightSearchables(System.Collections.Generic.IEnumerable<object> searchables);
        void RemoveProvider(Orc.Search.ISearchHighlightProvider provider);
        void ResetHighlights();
    }
    public static class ISearchHighlightServiceExtensions { }
    public interface ISearchHistoryService
    {
        System.Collections.Generic.IEnumerable<string> GetLastSearchQueries(string prefix, int count = 5);
    }
    public interface ISearchNavigationService
    {
        void Navigate(object searchable);
    }
    public interface ISearchQueryService
    {
        string GetSearchQuery(params Orc.Search.ISearchableMetadataValue[] searchableMetadataValues);
        string GetSearchQuery(string filter, System.Collections.Generic.IEnumerable<Orc.Search.ISearchableMetadata> searchableMetadatas);
    }
    public interface ISearchService
    {
        int IndexedObjectCount { get; }
        event System.EventHandler<Orc.Search.SearchEventArgs> Searched;
        event System.EventHandler<Orc.Search.SearchEventArgs> Searching;
        event System.EventHandler<System.EventArgs> Updated;
        event System.EventHandler<System.EventArgs> Updating;
        void AddObjects(System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> searchables);
        void ClearAllObjects();
        System.Collections.Generic.IEnumerable<Orc.Search.ISearchableMetadata> GetSearchableMetadata();
        void RemoveObjects(System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> searchables);
        System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> Search(string filter, int maxResults = 50);
    }
    public static class ISearchServiceExtensions { }
    public interface ISearchable : Orc.Metadata.IObjectWithMetadata { }
    public interface ISearchableMetadata : Orc.Metadata.IMetadata
    {
        bool Analyze { get; set; }
        string SearchName { get; set; }
    }
    public interface ISearchableMetadataValue
    {
        Orc.Search.ISearchableMetadata Metadata { get; }
        string Value { get; }
    }
    public class InMemorySearchService : Orc.Search.SearchServiceBase
    {
        public InMemorySearchService(Orc.Search.ISearchQueryService searchQueryService) { }
        protected override Lucene.Net.Store.Directory GetDirectory() { }
    }
    public class ReflectionSearchable : Orc.Search.Searchable
    {
        public ReflectionSearchable(object instance) { }
    }
    public static class SearchDefaults
    {
        public const int DefaultResults = 50;
    }
    public class SearchEventArgs : System.EventArgs
    {
        public SearchEventArgs(string filter, System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> results) { }
        public string Filter { get; }
        public System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> Results { get; }
    }
    public abstract class SearchHighlightProviderBase : Orc.Search.ISearchHighlightProvider
    {
        protected SearchHighlightProviderBase() { }
        public virtual void HighlightSearchable(object searchable) { }
        public virtual void ResetHighlight() { }
    }
    public class SearchHighlightService : Orc.Search.ISearchHighlightService
    {
        public SearchHighlightService(Orc.Search.ISearchService searchService) { }
        public event System.EventHandler<System.EventArgs> Highlighted;
        public event System.EventHandler<System.EventArgs> Highlighting;
        public void AddProvider(Orc.Search.ISearchHighlightProvider provider) { }
        public void HighlightSearchables(System.Collections.Generic.IEnumerable<object> searchables) { }
        public void RemoveProvider(Orc.Search.ISearchHighlightProvider provider) { }
        public void ResetHighlights() { }
    }
    public class SearchHistory : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData SearchHistoryElementsProperty;
        public SearchHistory() { }
        public System.Collections.Generic.List<Orc.Search.SearchHistoryElement> SearchHistoryElements { get; }
    }
    public class SearchHistoryElement : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData CountProperty;
        public static readonly Catel.Data.PropertyData EverFoundResultsProperty;
        public static readonly Catel.Data.PropertyData FilterLowerCaseProperty;
        public static readonly Catel.Data.PropertyData FilterProperty;
        public SearchHistoryElement() { }
        public int Count { get; set; }
        public bool EverFoundResults { get; set; }
        public string Filter { get; set; }
        public string FilterLowerCase { get; }
        protected override void OnDeserialized() { }
    }
    public class SearchHistoryService : Orc.Search.ISearchHistoryService
    {
        public SearchHistoryService(Orc.Search.ISearchService searchService, Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer, Catel.Services.IAppDataService appDataService, Orc.FileSystem.IDirectoryService directoryService) { }
        public System.Collections.Generic.IEnumerable<string> GetLastSearchQueries(string prefix, int count = 5) { }
    }
    public class SearchQueryService : Orc.Search.ISearchQueryService
    {
        public SearchQueryService() { }
        public string GetSearchQuery(params Orc.Search.ISearchableMetadataValue[] searchableMetadataValues) { }
        public string GetSearchQuery(string filter, System.Collections.Generic.IEnumerable<Orc.Search.ISearchableMetadata> searchableMetadatas) { }
    }
    public abstract class SearchServiceBase : Orc.Search.ISearchService
    {
        protected SearchServiceBase(Orc.Search.ISearchQueryService searchQueryService) { }
        public int IndexedObjectCount { get; }
        public event System.EventHandler<Orc.Search.SearchEventArgs> Searched;
        public event System.EventHandler<Orc.Search.SearchEventArgs> Searching;
        public event System.EventHandler<System.EventArgs> Updated;
        public event System.EventHandler<System.EventArgs> Updating;
        public virtual void AddObjects(System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> searchables) { }
        public void ClearAllObjects() { }
        protected virtual Lucene.Net.Index.IndexWriterConfig CreateIndexWriterConfig(Lucene.Net.Analysis.Analyzer analyzer) { }
        protected abstract Lucene.Net.Store.Directory GetDirectory();
        public virtual System.Collections.Generic.IEnumerable<Orc.Search.ISearchableMetadata> GetSearchableMetadata() { }
        public virtual void RemoveObjects(System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> searchables) { }
        public virtual System.Collections.Generic.IEnumerable<Orc.Search.ISearchable> Search(string filter, int maxResults = 50) { }
    }
    public class Searchable : Orc.Metadata.ObjectWithMetadata, Orc.Metadata.IObjectWithMetadata, Orc.Search.ISearchable
    {
        public Searchable(object instance, Orc.Metadata.IMetadataCollection metadataCollection) { }
    }
    public class SearchableMetadata : Orc.Metadata.ReflectionMetadata, Orc.Metadata.IMetadata, Orc.Search.ISearchableMetadata
    {
        public SearchableMetadata(System.Reflection.PropertyInfo propertyInfo) { }
        public bool Analyze { get; set; }
        public string SearchName { get; set; }
    }
    public class SearchableMetadataValue : Orc.Search.ISearchableMetadataValue
    {
        public SearchableMetadataValue(Orc.Search.ISearchableMetadata metadata, string value) { }
        public Orc.Search.ISearchableMetadata Metadata { get; }
        public string Value { get; }
    }
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class SearchablePropertyAttribute : System.Attribute
    {
        public SearchablePropertyAttribute() { }
        public bool Analyze { get; set; }
        public string SearchName { get; set; }
    }
    public static class StringExtensions
    {
        public static string ExtractRegexString(this string filter) { }
        public static bool IsValidOrcSearchFilter(this string filter) { }
        public static bool IsValidRegexPattern(this string pattern) { }
        public static string PrepareOrcSearchFilter(this string filter) { }
    }
}