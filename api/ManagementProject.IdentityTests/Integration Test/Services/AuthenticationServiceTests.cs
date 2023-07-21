using ManagementProject.Api;
using ManagementProject.Identity.Entity;
using ManagementProject.Identity.JwtModel;
using ManagementProject.Identity.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ManagementProject.Identity.Integration_Test.Services.Tests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private static TestServer _testServer;
        private static HttpClient _httpClient;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var configuration = BuildConfiguration();
            ConfigureLogger(configuration);

            var builder = new WebHostBuilder()
                .UseSerilog()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    // Replace the existing ManagementProjectIdentityDbContext registration
                    // with an in-memory database provider for testing purposes
                    services.AddDbContext<ManagementProjectIdentityDbContext>(options =>
                    {
                        options.UseSqlServer("Server=localhost;Database=ManagementProjectIdentityDb;Trusted_Connection=True;MultipleActiveResultSets=True;");
                    });
                });

            _testServer = new TestServer(builder);
            _httpClient = _testServer.CreateClient();

            // Set up the database context
            using var scope = _testServer.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ManagementProjectIdentityDbContext>();
            dbContext.Database.EnsureCreated(); // Create the in-memory database
        }

        [TestMethod]
        public async Task AuthenticateAsync_Check()
        {
            // Arrange
            using var scope = _testServer.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var jwtSettings = scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>();
            jwtSettings.Value.Key = "84322CFB66934ECC86D547C5CF4F2EFC";
            jwtSettings.Value.Issuer = "localhost";
            jwtSettings.Value.Audience = "localhost";
            jwtSettings.Value.DurationInMinutes = 60;
            var authenticationService = new AuthenticationService(userManager, jwtSettings);

            // Act
            var result = await authenticationService.AuthenticateAsync("admin", "admin");

            // Assert
            Assert.IsTrue(result.Token != null);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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