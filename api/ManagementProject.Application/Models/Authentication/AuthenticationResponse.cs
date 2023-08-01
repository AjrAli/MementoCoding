using ManagementProject.Application.Features.Response;
using System.Collections.Generic;

namespace ManagementProject.Application.Models.Authentication
{
    public class AuthenticationResponse : BaseResponse
    {
        public AuthenticationResponse() : base()
        {

        }
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Role { get;set; }
        public string? Email { get; set; }
        public string? Token { get; set; }

    }
}
