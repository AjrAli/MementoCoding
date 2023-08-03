using ManagementProject.Application.Features.Response;
using System.Collections.Generic;

namespace ManagementProject.Application.Models.Account
{
    public class AccountResponse : BaseResponse
    {
        public AccountResponse() : base()
        {

        }
        public string? Id { get; set; }
        public string? Token { get; set; }
    }
}
