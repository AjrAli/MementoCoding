using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using ManagementProject.Application.Features.PipelineBehaviours;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Students.Queries.GetStudent;
using ManagementProject.Application.Features.Students.Queries.GetStudents;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Events;

namespace ManagementProject.Api.Tests.Integration_Test.MediatR.Queries
{
    [TestClass]
    public class StudentQueriesTests : IDisposable
    {
        private IHost? _host;
        private HttpClient? _httpClient;


        [TestInitialize]
        public async Task SetupAsync()
        {
            var configuration = BuildConfiguration();
            ConfigureLogger(configuration);

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.ConfigureServices(ConfigureDependencies);
                    webHost.UseStartup<Startup>();
                    // Add TestServer
                    webHost.UseTestServer();
                })
                .UseSerilog();
            _host = await hostBuilder.StartAsync();
            _httpClient = _host.GetTestClient();

            // Set up the database context
            await InitializeDatabase();
        }

        [TestMethod]
        public async Task Should_Return_Correct_StudentDto()
        {
            // Arrange
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            var dto = new GetStudentDto()
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 1,
                Parentname = "test"
            };
            if (dbContext?.Students?.Count() == 0)
            {
                await dbContext.Students.AddAsync(new Student()
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    SchoolId = 5,
                    School = new School()
                        { Id = 1, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
                });
                await dbContext.SaveChangesAsync();
            }

            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();


            // Act
            var result = await mediator.Send(new GetStudentQuery
            {
                StudentId = 1
            });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrEmpty(result.StudentDto?.FirstName));
            Assert.IsTrue(result.StudentDto.Equals(dto));
        }

        [TestMethod]
        public async Task Should_Return_Correct_List_StudentDto()
        {
            // Arrange
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            var listDto = new List<GetStudentsDto>()
            {
                new GetStudentsDto()
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "Test",
                    Adress = "MyAdress",
                    Age = 10,
                    SchoolId = 5,
                    Parentname = "test"
                },
                new GetStudentsDto()
                {
                    Id = 6,
                    FirstName = "Test6",
                    LastName = "Test6",
                    Adress = "MyAdress6",
                    Age = 16,
                    SchoolId = 10,
                    Parentname = "test"
                }
            };
            if (dbContext?.Students?.Count() == 0)
            {
                await dbContext.Students.AddRangeAsync(new List<Student>()
                {
                    new Student()
                    {
                        Id = 3,
                        FirstName = "Test",
                        LastName = "Test",
                        Adress = "MyAdress",
                        Age = 10,
                        SchoolId = 5,
                        School = new School()
                            { Id = 5, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
                    },
                    new Student()
                    {
                        Id = 6,
                        FirstName = "Test6",
                        LastName = "Test6",
                        Adress = "MyAdress6",
                        Age = 16,
                        SchoolId = 10,
                        School = new School()
                            { Id = 10, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
                    }
                });
                await dbContext.SaveChangesAsync();
            }

            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();


            // Act
            var result = await mediator.Send(new GetStudentsQuery
            {
                Options = null
            });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrEmpty(result.StudentsDto.FirstOrDefault()?.FirstName));
            Assert.IsTrue(result.StudentsDto.SequenceEqual(listDto));
        }

        [TestCleanup]
        public void ResetDbSetStudents()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            if (!(dbContext?.Students?.Count() > 0)) return;
            dbContext.Students.RemoveRange(dbContext.Students);
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

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddDbContext<ManagementProjectDbContext>(options => { options.UseInMemoryDatabase("FakeDB"); });
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