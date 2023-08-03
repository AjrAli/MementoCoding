using ManagementProject.Application.Features.Students;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Application.Models.Account.Command
{
    public class CreateSimpleUserCommand : IRequest<AccountResponse>
    {
        public AccountDto Account { get; set; } = new AccountDto();
    }
}
