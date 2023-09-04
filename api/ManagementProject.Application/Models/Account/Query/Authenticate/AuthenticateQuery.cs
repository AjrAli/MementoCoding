using ManagementProject.Application.Contracts.MediatR.Query;

namespace ManagementProject.Application.Models.Account.Query.Authenticate
{
    public class AuthenticateQuery : IQuery<AccountResponse>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
