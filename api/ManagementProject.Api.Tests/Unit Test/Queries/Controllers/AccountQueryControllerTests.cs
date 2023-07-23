using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ManagementProject.Api.Controllers.Queries;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Models.Authentication;
using ManagementProject.Identity.Entity;
using ManagementProject.Identity.JwtModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationService = ManagementProject.Identity.Services.AuthenticationService;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ManagementProject.Api.Tests.Unit_Test.Queries.Controllers
{
    [TestClass]
    public class AccountQueryControllerTests
    {
        private readonly ILogger<AccountQueryController> _logger = Mock.Of<ILogger<AccountQueryController>>();

        [TestMethod]
        public async Task AuthenticateAsync_ReturnToken()
        {
            // Arrange
            var request = new AuthenticationRequest()
            {
                Username = "admin",
                Password = "admin"
            };
            var accountController = InitAccountController();

            // Act
            var resultAuthCall = await accountController.AuthenticateAsync(request);
            var val = (resultAuthCall.Result as OkObjectResult)?.Value as AuthenticationResponse;

            // Assert
            Assert.IsTrue(val?.Token != null);
        }
        [TestMethod]
        public async Task AuthenticateAsync_ReturnBadRequest()
        {
            // Arrange
            AuthenticationRequest request = null;
            var accountController = InitAccountController();

            // Act
            var resultAuthCall = await accountController.AuthenticateAsync(request);
            var result = resultAuthCall?.Result;

            // Assert
            Assert.IsTrue(result is BadRequestObjectResult);
            var success = (((result as BadRequestObjectResult)?.Value) as ErrorResponse)?.Success;
            Assert.IsTrue(!success);
        }

        private AccountQueryController InitAccountController()
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
            userManager.Setup(m => m.FindByNameAsync("admin")).ReturnsAsync(user);
            userManager.Setup(u => u.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            });
            userManager.Setup(m => m.CheckPasswordAsync(user, "admin")).ReturnsAsync(true);

            var jwtSettings = new JwtSettings
            {
                Key = "84322CFB66934ECC86D547C5CF4F2EFC",
                Issuer = "localhost",
                Audience = "localhost",
                DurationInMinutes = 60
            };
            var options = Options.Create(jwtSettings);

            var authenticationService = new AuthenticationService(userManager.Object, options);
            return new AccountQueryController(authenticationService, _logger);
        }



        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = Mock.Of<IUserStore<TUser>>();
            var userManager = new Mock<UserManager<TUser>>(store, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<TUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return userManager;
        }

        public static Mock<RoleManager<TRole>> MockRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? Mock.Of<IRoleStore<TRole>>();
            var roleManager = new Mock<RoleManager<TRole>>(store, new List<IRoleValidator<TRole>>(), Mock.Of<ILookupNormalizer>(), Mock.Of<IdentityErrorDescriber>(), null);
            return roleManager;
        }
    }
}