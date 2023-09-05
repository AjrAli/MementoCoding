using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Contracts.Identity;
using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Models.Account.Command
{
    public class CreateSimpleUserCommandHandler : ICommandHandler<CreateSimpleUserCommand, AccountResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public CreateSimpleUserCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AccountResponse> Handle(CreateSimpleUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.CreateSimpleUserAsync(request);
            response.Message = $"User {request.Account.Username} successfully connected";
            return response;
        }
    }
}
