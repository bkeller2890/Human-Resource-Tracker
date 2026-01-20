using HRTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HRTracker.HostedServices;

/// <summary>
/// One-shot hosted service that applies migrations and seeds the database on startup.
/// Runs once and stops.
/// </summary>
public class DbInitializerHostedService : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DbInitializerHostedService> _logger;

    public DbInitializerHostedService(IServiceProvider services, ILogger<DbInitializerHostedService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<HRDbContext>();
            _logger.LogInformation("Applying migrations and seeding database...");
            await context.Database.MigrateAsync(cancellationToken);
            DbSeeder.Seed(context);
            _logger.LogInformation("Database initialization completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            // don't rethrow; we don't want to crash the host for a non-fatal seeding error
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
