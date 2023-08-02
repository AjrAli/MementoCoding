using ManagementProject.Application.Contracts.Identity;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Models.Account.Query.Authenticate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Models.Account.Command
{
    public class CreateSimpleUserCommandHandler : IRequestHandler<CreateSimpleUserCommand, AccountResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public CreateSimpleUserCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AccountResponse> Handle(CreateSimpleUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.CreateSimpleUserAsync(request);
            response.Message = $"User {response.UserName} successfully connected";
            return response;
        }
    }
}
