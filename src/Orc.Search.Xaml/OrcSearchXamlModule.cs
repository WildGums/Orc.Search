namespace Orc;

using Catel.Services;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcSearchXamlModule
{
    public static IServiceCollection AddOrcSearchXaml(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Search.Xaml", "Orc.Search.Properties", "Resources"));

        return serviceCollection;
    }
}
