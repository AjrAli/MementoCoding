using SchoolProject.Management.Application.Models.Authentication;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(string username, string password);
    }
}
