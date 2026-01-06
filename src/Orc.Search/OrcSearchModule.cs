namespace Orc
{
    using Catel.Services;
    using Catel.ThirdPartyNotices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orc.Search;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcSearchModule
    {
        public static IServiceCollection AddOrcSearch(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ISearchService, InMemorySearchService>();
            serviceCollection.TryAddSingleton<ISearchHistoryService, SearchHistoryService>();
            serviceCollection.TryAddSingleton<ISearchHighlightService, SearchHighlightService>();
            serviceCollection.TryAddSingleton<ISearchNavigationService, DummySearchNavigationService>();
            serviceCollection.TryAddSingleton<ISearchQueryService, SearchQueryService>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Search", "Orc.Search.Properties", "Resources"));

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.Search", "https://github.com/wildgums/orc.csv"));
            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Lucene.NET", "https://github.com/apache/lucenenet", "Orc.Search", "Orc.Search", "Resources.ThirdPartyNotices.lucene.net.txt"));

            return serviceCollection;
        }
    }
}
