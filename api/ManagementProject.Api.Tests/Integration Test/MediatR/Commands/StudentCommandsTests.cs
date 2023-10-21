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
using ManagementProject.Application.Features.Students;
using ManagementProject.Application.Features.Students.Commands.CreateStudent;
using ManagementProject.Application.Features.Students.Commands.DeleteStudent;
using ManagementProject.Application.Features.Students.Commands.UpdateStudent;
using ManagementProject.Application.Profiles.Students;
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
    public class StudentCommandsTests : IDisposable
    {
        private IHost? _host;
        private HttpClient? _httpClient;

        private readonly IMapper _mapper =
            new MapperConfiguration(x => x.AddProfile<StudentMappingProfile>()).CreateMapper();

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
        public async Task CreateStudentCommand_Should_Return_Correct_Added_StudentDto()
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
            var studentDto = new StudentDto()
            {
                Id = 1,
                FirstName = "test",
                LastName = "town",
                Adress = "address",
                Age = 10,
                SchoolId = schoolToDelete.Id
            };
            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new CreateStudentCommand
            {
                Student = studentDto
            });

            var newStudentAdded = dbContext.Students.FirstOrDefault(x => x.Id == studentDto.Id);
            var newStudentAddedAsStudentDto = _mapper.Map<StudentDto>(newStudentAdded);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(newStudentAddedAsStudentDto.Equals(studentDto));
        }

        [TestMethod]
        public async Task DeleteStudentCommand_Should_Return_Correct_Null()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            var studentToDelete = new Student()
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 1,
                School = new School()
                    { Id = 1, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
            };
            await dbContext.Students.AddAsync(studentToDelete);
            await dbContext.SaveChangesAsync();
            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new DeleteStudentCommand
            {
                StudentId = studentToDelete.Id
            });

            var newStudentAdded = dbContext.Students.FirstOrDefault(x => x.Id == studentToDelete.Id);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(newStudentAdded);
        }

        [TestMethod]
        public async Task UpdateStudentCommand_Should_Return_Correct_Updated_Student()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            var studentToUpdate = new Student()
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Adress = "MyAdress",
                Age = 10,
                SchoolId = 2,
                School = new School()
                    { Id = 2, Name = "test", Town = "town", Adress = "adres", Description = "desc" }
            };
            var studentDto = new StudentDto()
            {
                Id = 1,
                FirstName = "test",
                LastName = "town",
                Adress = "address",
                Age = 10,
                SchoolId = studentToUpdate.Id
            };
            await dbContext.Students.AddAsync(studentToUpdate);
            await dbContext.SaveChangesAsync();
            var mediator = scope?.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new UpdateStudentCommand
            {
                Student = studentDto
            });

            var newStudentAdded = dbContext.Students.FirstOrDefault(x => x.Id == studentToUpdate.Id);
            var newStudentAddedAsStudentDto = _mapper.Map<StudentDto>(newStudentAdded);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(newStudentAddedAsStudentDto.Equals(studentDto));
        }

        [TestCleanup]
        public void ResetDbSetStudents()
        {
            using var scope = _host?.Services.CreateScope();
            var dbContext = scope?.ServiceProvider.GetRequiredService<ManagementProjectDbContext>();
            if (!(dbContext?.Students?.Count() > 0)) return;
            dbContext.Students.RemoveRange(dbContext.Students);
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