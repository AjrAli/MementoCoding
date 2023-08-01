using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ManagementProject.Identity.Entity;
using ManagementProject.Identity.Seed;
using ManagementProject.Persistence.Context;
using ManagementProject.Persistence.Seed;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace ManagementProject.Api
{
    public static class Program
    {
        public async static Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .ReadFrom.Configuration(config)
                        .CreateBootstrapLogger();
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await CreateRoles.SeedAsync(roleManager);
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    await CreateFirstUser.SeedAsync(userManager);
                    var dbContext = services.GetRequiredService<ManagementProjectDbContext>();
                    await DatabaseSeeder.SeedAsync(dbContext);
                    Log.Information("Starting web host");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while starting the application");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }

            await host.RunAsync();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
