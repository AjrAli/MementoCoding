using ManagementProject.Application.Models.Account;
using ManagementProject.Application.Models.Account.Query.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ManagementProject.Api.Controllers.Queries
{
    [ApiController]
    public class AccountQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountQueryController> _logger;
        public AccountQueryController(IMediator mediator,
                                ILogger<AccountQueryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateQuery request)
        {
            AccountResponse? dataReponse = await _mediator.Send(request);
            return Ok(dataReponse);
        }
    }
}
