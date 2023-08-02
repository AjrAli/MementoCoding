using ManagementProject.Application.Contracts.Identity;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Models.Account.Query.Authenticate
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AccountResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AccountResponse> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            if (request?.Username == null || request?.Password == null)
                throw new BadRequestException($"One of the credentials given is empty");
            var response = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
            response.Message = $"User {response.UserName} successfully connected";
            return response;
        }
    }
}
