using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Acropolis.Shared.Extensions;

public static class ServiceProviderExtensions
{
    public static TOptions GetOptions<TOptions>(this IServiceProvider sp)
        where TOptions : class
    {
        return sp.GetRequiredService<IOptions<TOptions>>().Value;
    }
}