using Catel.IoC;
using Catel.Services;
using Catel.Services.Models;
using Orc.Metadata;
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

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.Search", "Orc.Search.Properties", "Resources"));
    }
}