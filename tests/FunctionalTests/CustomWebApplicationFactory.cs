using LocationSearch.API;
using LocationSearch.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FunctionalTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddEntityFrameworkInMemoryDatabase();

                // Create a new service provider.
                var provider = services
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<LocationContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });


                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<LocationContext>();
                    var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        LocationContextSeed.SeedAsync(db, loggerFactory).Wait();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the " +
                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}
