using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Management.Application.Contracts.Identity;
using SchoolProject.Management.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(string username, string password)
        {
            var response = await _authenticationService.AuthenticateAsync(username, password);
            if(response != null && response.Token != null)
            {
                Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1).AddMinutes(-5),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });
            }
            return Ok(response);
        }
    }
}
