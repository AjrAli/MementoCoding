using ManagementProject.Api.Controllers.Queries;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Schools.Queries.GetSchool;
using ManagementProject.Application.Models.Account;
using ManagementProject.Application.Models.Account.Query.Authenticate;
using ManagementProject.Identity.Entity;
using ManagementProject.Identity.JwtModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AuthenticationService = ManagementProject.Identity.Services.AuthenticationService;

namespace ManagementProject.Api.Tests.Unit_Test.Queries.Controllers
{
    [TestClass]
    public class AccountQueryControllerTests
    {
        private readonly ILogger<AccountQueryController> _logger = Substitute.For<ILogger<AccountQueryController>>();

        [TestMethod]
        public async Task AuthenticateAsync_ReturnToken()
        {
            // Arrange
            var request = new AuthenticateQuery()
            {
                Username = "admin",
                Password = "admin"
            };
            var accountController = InitAccountController();

            // Act
            var resultAuthCall = await accountController.AuthenticateAsync(request);

            var result = resultAuthCall as OkObjectResult;
            var val = result?.Value as AccountResponse;

            // Assert
            Assert.IsTrue(val?.Token != null);
        }
        [TestMethod]
        public async Task AuthenticateAsync_ReturnBadRequest()
        {
            // Arrange
            AuthenticateQuery request = new ();
            var accountController = InitAccountController();

            // Act && Assert
            await Assert.ThrowsExceptionAsync<BadRequestException>(async () =>
            {
                await accountController.AuthenticateAsync(request);
            });
        }

        private AccountQueryController InitAccountController()
        {
            IMediator mediatorMock = MockMediatorAuthenticateQuery();
            return new AccountQueryController(mediatorMock, _logger);
        }

        private static IMediator MockMediatorAuthenticateQuery()
        {
            var mediatorMock = Substitute.For<IMediator> ();
            mediatorMock.Send(Arg.Any<AuthenticateQuery>(), default).Returns(x =>
            {
                return InitAuthenticateQueryHandler().Handle(x.Arg<AuthenticateQuery>(), x.Arg<CancellationToken>());
            });
            return mediatorMock;
        }

        private static AuthenticateQueryHandler InitAuthenticateQueryHandler()
        {

            var user = new ApplicationUser
            {
                Id = "3c112624-eff3-41bf-9359-2d4ca50ce130",
                FirstName = "Ali",
                UserName = "admin",
                LastName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "ali@gmail.com",
                NormalizedEmail = "ALI@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEIyLWbcyRCIGNEFCvoO2hGo6unGbE0ZR+Xst4uJeN4v9W725q2XV2W0Wz/IabWYvpg==",
                SecurityStamp = "N2DPMR4R6YT4POHKY67WYGY2D7SWTQEB",
                ConcurrencyStamp = "4719d404-cb9a-4c3d-acb2-73a9adc9f7ff",
                LockoutEnabled = true
            };

            var userManager = MockUserManager<ApplicationUser>();
            userManager.FindByNameAsync("admin").Returns(user);
            userManager.GetClaimsAsync(user).Returns(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            });
            userManager.GetRolesAsync(user).Returns(new List<string>() { "Administrator" });
            userManager.CheckPasswordAsync(user, "admin").Returns(true);

            var jwtSettings = new JwtSettings
            {
                Key = "84322CFB66934ECC86D547C5CF4F2EFC",
                Issuer = "localhost",
                Audience = "localhost",
                DurationInMinutes = 60
            };
            var options = Options.Create(jwtSettings);

            var authenticationService = new AuthenticationService(userManager, options);
            return new AuthenticateQueryHandler(authenticationService);
        }

        public static UserManager<TUser> MockUserManager<TUser>() where TUser : class
        {
            var store = Substitute.For<IUserStore<TUser>>();
            var userManager = Substitute.For<UserManager<TUser>>(store, null, null, null, null, null, null, null, null);
            userManager.UserValidators.Add(new UserValidator<TUser>());
            userManager.PasswordValidators.Add(new PasswordValidator<TUser>());
            return userManager;
        }

        public static RoleManager<TRole> MockRoleManager<TRole>(IRoleStore<TRole>? store = null) where TRole : class
        {
            store ??= Substitute.For<IRoleStore<TRole>>();
            var roleManager = Substitute.For<RoleManager<TRole>>(store, new List<IRoleValidator<TRole>>(), Substitute.For<ILookupNormalizer>(), Substitute.For<IdentityErrorDescriber>(), null);
            return roleManager;
        }
    }
}