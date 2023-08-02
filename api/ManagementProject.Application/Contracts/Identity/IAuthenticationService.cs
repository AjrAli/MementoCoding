using ManagementProject.Application.Models.Account;
using ManagementProject.Application.Models.Account.Command;
using System.Threading.Tasks;

namespace ManagementProject.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AccountResponse> AuthenticateAsync(string username, string password);
        Task<AccountResponse> CreateSimpleUserAsync(CreateSimpleUserCommand request);
    }
}
