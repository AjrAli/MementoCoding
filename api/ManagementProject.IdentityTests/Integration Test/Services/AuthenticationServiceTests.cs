using ManagementProject.Api;
using ManagementProject.Identity.JwtModel;
using ManagementProject.Identity.Services;
using ManagementProject.Persistence.Context;
using ManagementProject.Persistence.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Events;

namespace ManagementProject.IdentityTests.Integration_Test.Services
{
    [TestClass]
    public class AuthenticationServiceTests : IDisposable
    {
        private IHost? _host;
        private HttpClient? _httpClient;

        [TestInitialize]
        public async Task Setup()
        {
            var configuration = BuildConfiguration();
            ConfigureLogger(configuration);

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseStartup<Startup>();
                    webHost.ConfigureServices(services =>
                    {
                        services.AddOptions<JwtSettings>().Bind(configuration.GetSection("JwtSettings"));
                        services.AddDbContext<ManagementProjectDbContext>(options =>
                        {
                            options.UseSqlServer(
                                "Server=localhost;Database=MementoCodingDB;Trusted_Connection=True;MultipleActiveResultSets=True;");
                        });
                    });
                    // Add TestServer
                    webHost.UseTestServer();
                })
                .UseSerilog();
            _host = await hostBuilder.StartAsync();
            _httpClient = _host.GetTestClient();

            // Set up the database context
            using var scope = _host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            await dbContext.Database.OpenConnectionAsync(); // Open the database connection
            await dbContext.Database.EnsureCreatedAsync(); // Create the  database schema
        }

        [TestMethod]
        public async Task AuthenticateAsync_ValidUser_ReturnsToken()
        {
            // Arrange
            using var scope = _host.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var jwtSettings = scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();


            var authenticationService = new AuthenticationService(userManager, jwtSettings);

            // Act
            var result = await authenticationService.AuthenticateAsync("admin", "admin");

            // Assert
            Assert.IsNotNull(result.Token);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Token)); // Vous pouvez ajouter d'autres assertions ici
        }

        // ...

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }

        public async Task DisposeAsync()
        {
            _httpClient?.Dispose();

            if (_host != null)
            {
                using var scope = _host.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
                await dbContext.Database.CloseConnectionAsync(); // Close the in-memory database connection
                _host.Dispose();
            }
        }

        private static IConfiguration BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Use appsettings.{environment}.json
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Use the default appsettings.json
                .Build();
        }

        private static void ConfigureLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateBootstrapLogger();
        }
    }
}