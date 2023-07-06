using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Management.Application.Contracts.Identity;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Controllers.Queries
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
            try
            {
                if (!string.IsNullOrEmpty(request?.Username) && !string.IsNullOrEmpty(request?.Password))
                {
                    var response = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
                    if (response != null && response.Token != null)
                    {
                        Response?.Cookies?.Append("X-Access-Token", response.Token, new CookieOptions()
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(1).AddMinutes(-5),
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        });
                        return Ok(response);
                    }
                }
                throw new BadRequestException("Invalid username or password.");

            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);
                var errorResponse = ex.CreateErrorResponse();
                return BadRequest(errorResponse);
            }
        }
    }
}
