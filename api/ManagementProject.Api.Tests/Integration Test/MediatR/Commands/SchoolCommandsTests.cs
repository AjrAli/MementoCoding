using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using ManagementProject.Application.Features.PipelineBehaviours;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Schools;
using ManagementProject.Application.Features.Schools.Commands.CreateSchool;
using ManagementProject.Application.Features.Schools.Commands.DeleteSchool;
using ManagementProject.Application.Features.Schools.Commands.UpdateSchool;
using ManagementProject.Application.Profiles.Schools;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Events;

namespace ManagementProject.Api.Tests.Integration_Test.MediatR.Commands
{
    [TestClass]
    public class SchoolCommandsTests : IDisposable
    {
        private IHost? _host;
        private HttpClient? _httpClient;

        private readonly IMapper _mapper =
            new MapperConfiguration(x => x.AddProfile<SchoolMappingProfile>()).CreateMapper();

        [TestInitialize]
        public async Task InitializeAsync()
        {
            var configuration = BuildConfiguration();
            ConfigureLogger(configuration);

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.ConfigureServices(ConfigureDependencies);
                    webHost.UseStartup<Startup>();
                    webHost.UseTestServer();
                })
                .UseSerilog();
            _host = await hostBuilder.StartAsync();
            _httpClient = _host.GetTestClient();

            await InitializeDatabase();
        }

        [TestMethod]
        public async Task CreateSchoolCommand_Should_Return_Correct_Added_SchoolDto()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            var schoolDto = new SchoolDto()
            {
                Id = 1,
                Name = "test",
                Town = "town",
                Adress = "address",
                Description = "desc"
            };
            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new CreateSchoolCommand
            {
                School = schoolDto
            });

            var newSchoolAdded = dbContext.Schools.FirstOrDefault(x => x.Id == schoolDto.Id);
            var newSchoolAddedAsSchoolDto = _mapper.Map<SchoolDto>(newSchoolAdded);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(newSchoolAddedAsSchoolDto.Equals(schoolDto));
        }

        [TestMethod]
        public async Task DeleteSchoolCommand_Should_Return_Correct_Null()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            var schoolToDelete = new School()
            {
                Id = 1,
                Name = "test",
                Town = "town",
                Adress = "address",
                Description = "desc"
            };
            await dbContext.Schools.AddAsync(schoolToDelete);
            await dbContext.SaveChangesAsync();
            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new DeleteSchoolCommand
            {
                SchoolId = schoolToDelete.Id
            });

            var newSchoolAdded = dbContext.Schools.FirstOrDefault(x => x.Id == schoolToDelete.Id);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(newSchoolAdded);
        }

        [TestMethod]
        public async Task UpdateSchoolCommand_Should_Return_Correct_Updated_School()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            var schoolToUpdate = new School()
            {
                Id = 1,
                Name = "test",
                Town = "town",
                Adress = "address",
                Description = "desc"
            };
            var schoolDto = new SchoolDto()
            {
                Id = schoolToUpdate.Id,
                Name = "testUpdated",
                Town = "townUpdated",
                Adress = "addressUpdated",
                Description = "descUpdated"
            };
            await dbContext.Schools.AddAsync(schoolToUpdate);
            await dbContext.SaveChangesAsync();
            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new UpdateSchoolCommand
            {
                School = schoolDto
            });

            var newSchoolAdded = dbContext.Schools.FirstOrDefault(x => x.Id == schoolToUpdate.Id);
            var newSchoolAddedAsSchoolDto = _mapper.Map<SchoolDto>(newSchoolAdded);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(newSchoolAddedAsSchoolDto.Equals(schoolDto));
        }

        [TestCleanup]
        public void ResetDbSetSchools()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            if (!(dbContext?.Schools?.Count() > 0)) return;
            dbContext.Schools.RemoveRange(dbContext.Schools);
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }

        private async Task DisposeAsync()
        {
            _httpClient?.Dispose();

            if (_host != null)
            {
                using var scope = _host.Services.CreateScope();
                await _host.StopAsync();
                _host.Dispose();
            }
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

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddDbContext<ManagementProjectDbContext>(options =>
            {
                options.UseInMemoryDatabase("FakeDB");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            services.AddTransient(typeof(IResponseFactory<>), typeof(ResponseFactory<>));
        }

        private async Task InitializeDatabase()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            await dbContext?.Database.EnsureCreatedAsync()!;
        }
    }
}