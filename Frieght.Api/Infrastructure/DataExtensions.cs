﻿using Frieght.Api.Repositories;
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

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // setitngs for docker container
        var connString = Environment.GetEnvironmentVariable("DefaultConnection"); // to retrieve connection from docker container environment variable   
        // var connString = configuration.GetConnectionString("DefaultConnection"); // to retrieve connection from configuration file like appsettings.json
        // Console.WriteLine($"DefaultConnection: {connString}");

        // var connString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<FrieghtDbContext>(options =>
            options.UseNpgsql(connString)) // Changed to UseNpgsql
            .AddScoped<ILoadRepository, LoadRepository>()
            .AddScoped<ICarrierRepository, CarrierRepository>();

        return services;
    }
}
