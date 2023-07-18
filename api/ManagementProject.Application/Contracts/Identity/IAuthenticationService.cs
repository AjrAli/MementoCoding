using ManagementProject.Application.Models.Authentication;
using System.Threading.Tasks;

namespace ManagementProject.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(string username, string password);
    }
}
