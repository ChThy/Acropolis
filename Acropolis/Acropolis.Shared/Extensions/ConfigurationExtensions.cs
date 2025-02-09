using Microsoft.Extensions.Configuration;

namespace Acropolis.Shared.Extensions;

public static class ConfigurationExtensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string sectionKey)
        => configuration.GetSection(sectionKey).Get<TOptions>() ?? throw new InvalidOperationException($"Failed to get configuration of type {typeof(TOptions)} with section key {sectionKey}");
}