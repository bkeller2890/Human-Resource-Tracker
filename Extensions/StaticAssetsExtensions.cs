using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace HRTracker.Extensions;

public static class StaticAssetsExtensions
{
    // Enables serving files from wwwroot and default files.
    public static void MapStaticAssets(this WebApplication app)
    {
        // Ensure static files and default files are served
        app.UseDefaultFiles();
        app.UseStaticFiles();
    }

    // Fluent no-op used after MapControllerRoute to keep call-site readable.
    public static IEndpointConventionBuilder WithStaticAssets(this IEndpointConventionBuilder builder)
    {
        return builder;
    }
}
