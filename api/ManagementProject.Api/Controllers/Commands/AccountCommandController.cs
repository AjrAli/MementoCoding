using ManagementProject.Application.Models.Account;
using ManagementProject.Application.Models.Account.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ManagementProject.Api.Controllers.Commands
{
    [ApiController]
    public class AccountCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountCommandController> _logger;
        public AccountCommandController(IMediator mediator,
                                ILogger<AccountCommandController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("create-simple-user")]
        public async Task<IActionResult> CreateSimpleUserAsync([FromBody] AccountDto accountDto)
        {
            AccountResponse? dataReponse = await _mediator.Send(new CreateSimpleUserCommand
            {
                Account = accountDto
            });
            return Ok(dataReponse);
        }
    }
}
