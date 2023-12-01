using Frieght.Api.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Frieght.Api.Infrastructure;

public static  class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FrieghtDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    // public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    // {
    //     var connString = configuration.GetConnectionString("DefaultConnection");
    //     services.AddSqlServer<FrieghtDbContext>(connString)
    //         .AddScoped<ILoadRepository, LoadRepository>()
    //         .AddScoped<ICarrierRepository, CarrierRepository>();

    //     return services;
    // }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<FrieghtDbContext>(options =>
            options.UseNpgsql(connString)) // Changed to UseNpgsql
            .AddScoped<ILoadRepository, LoadRepository>()
            .AddScoped<ICarrierRepository, CarrierRepository>()
            .AddScoped<IBidRepository, BidRepository>();

        return services;
    }
}
