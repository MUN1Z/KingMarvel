using KingMarvel.Database.Contexts;
using KingMarvel.IntegrationTests.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KingMarvel.IntegrationTests.Factories
{
    public class BaseWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        #region private members

        private const string VARIABLE_DB_CONNECTION_STRING = "DB_INTEGRATION_TESTS_CONNECTION_STRING";
        private const string DEFAULT_DB_CONNECTION_STRING = "TODO: GETFROM SETTINGS";

        #endregion

        #region protected methods implementations

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Is be called after the `ConfigureServices` from the Startup
            // which allows you to overwrite the DI with mocked instances
            builder.ConfigureTestServices(services =>
            {
                var descriptorContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<KingMarvelContext>));
                services.Remove(descriptorContext);

                var databaseConnectionString = Environment.GetEnvironmentVariable(VARIABLE_DB_CONNECTION_STRING);
                services.AddDbContext<KingMarvelContext>(optionsBuilder => optionsBuilder.UseSqlServer(string.IsNullOrEmpty(databaseConnectionString) ? DEFAULT_DB_CONNECTION_STRING : databaseConnectionString));

                services.AddScoped<IDatabaseTestManager, DatabaseTestManager>();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var databaseManager = scopedServices.GetRequiredService<IDatabaseTestManager>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<WebApplicationFactory<Program>>>();

                    try
                    {
                        var ctx = scope?.ServiceProvider.GetService<KingMarvelContext>();

                        ctx.Database.EnsureDeleted();
                        ctx.Database.EnsureCreated();

                        databaseManager.SeedTestData().Wait();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "", ex.Message);
                    }
                }
            });
        }

        #endregion
    }
}
