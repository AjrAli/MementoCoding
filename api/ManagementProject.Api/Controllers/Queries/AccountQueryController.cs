using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ManagementProject.Application.Contracts.Identity;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace ManagementProject.Api.Controllers.Queries
{
    [ApiController]
    public class AccountQueryController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AccountQueryController> _logger;
        public AccountQueryController(IAuthenticationService authenticationService,
                                 ILogger<AccountQueryController> logger)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            if(request.Username == null || request.Password == null)
                throw new NotFoundException($"One of the credentials given is empty");
            var response = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
            return Ok(response);
            #region Store token on a Cookie connection
            /******** ONLY FOR TEST WITHOUT USING Postman ********************/
            /*
                        _logger.LogInformation($"{DateTimeOffset.UtcNow.AddDays(1).AddMinutes(-5)}");
                        _logger.LogInformation($"{DateTimeOffset.Now.AddMinutes(2)}");
                        Response?.Cookies?.Append("X-Access-Token", response.Token, new CookieOptions()
                        {
                            Expires = DateTimeOffset.Now.AddMinutes(2),
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        });
            */
            #endregion

        }
    }
}
