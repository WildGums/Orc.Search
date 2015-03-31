using Catel.IoC;
using Orc.Search;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<ISearchService, InMemorySearchService>();
        serviceLocator.RegisterType<ISearchHistoryService, SearchHistoryService>();
        serviceLocator.RegisterType<ISearchHighlightService, SearchHighlightService>();
        serviceLocator.RegisterType<ISearchNavigationService, DummySearchNavigationService>();
        serviceLocator.RegisterType<ISearchQueryService, SearchQueryService>();
        serviceLocator.RegisterType<ISearchableParser, AttributeSearchableParser>();
        serviceLocator.RegisterType<ISearchableAdapter, ReflectionSearchableAdapter>();
    }
}