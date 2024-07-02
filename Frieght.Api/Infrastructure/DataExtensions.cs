using Frieght.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Frieght.Api.Infrastructure.Notifications;

namespace Frieght.Api.Infrastructure;

public static  class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FrieghtDbContext>();
        await dbContext.Database.MigrateAsync();

        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Log the exception and handle it as needed
            Console.WriteLine($"Migration failed: {ex.Message}");
        }
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // setitngs for docker container
        // var connString = Environment.GetEnvironmentVariable("DefaultConnection"); // to retrieve connection from docker container environment variable   
        
        var connString = configuration.GetConnectionString("DefaultConnection"); // to retrieve connection from configuration file like appsettings.json
        Console.WriteLine($"DefaultConnection: {connString}");

        services.AddDbContext<FrieghtDbContext>(options =>
            options.UseNpgsql(connString)) // Changed to UseNpgsql
            .AddScoped<ILoadRepository, LoadRepository>()
            .AddScoped<ICarrierRepository, CarrierRepository>()
            .AddScoped<IMessageSender, MessageSender>()
            .AddScoped<IBidRepository, BidRepository>();

        return services;
    }
}
