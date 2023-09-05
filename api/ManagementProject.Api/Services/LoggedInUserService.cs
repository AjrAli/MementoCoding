using Microsoft.AspNetCore.Http;
using ManagementProject.Persistence.Contracts;
using System.Security.Claims;

namespace ManagementProject.Api.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string? UserId { get; set; }
    }
}
