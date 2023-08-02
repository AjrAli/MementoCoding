using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Application.Models.Account.Query.Authenticate
{
    public class AuthenticateQuery : IRequest<AccountResponse>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
