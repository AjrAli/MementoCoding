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
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }

    }
}
