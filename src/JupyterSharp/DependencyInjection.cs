using Microsoft.Extensions.DependencyInjection;

namespace JupyterSharp;

public static class DependencyInjection
{
    public static IServiceCollection AddJupyterClient(this IServiceCollection services)
    {
        return services;
    }
}
