namespace Orc.Search.Tests;

using Catel;
using Microsoft.Extensions.DependencyInjection;
using Orc.FileSystem;
using Orc.Metadata;
using Orc.Search;
using Orc.Serialization.Json;

internal static class ServiceCollectionHelper
{
    public static IServiceCollection CreateServiceCollection()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging();
        serviceCollection.AddCatelCore();
        serviceCollection.AddCatelMvvm();
        serviceCollection.AddOrcFileSystem();
        serviceCollection.AddOrcMetadata();
        serviceCollection.AddOrcSearch();
        serviceCollection.AddOrcSearchXaml();
        serviceCollection.AddOrcSerializationJson();

        return serviceCollection;
    }
}
