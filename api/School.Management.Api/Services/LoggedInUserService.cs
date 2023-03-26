using Microsoft.AspNetCore.Http;
using SchoolProject.Management.Application.Contracts;
using System.Security.Claims;

namespace SchoolProject.Management.Api.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string? UserId { get; }
    }
}
