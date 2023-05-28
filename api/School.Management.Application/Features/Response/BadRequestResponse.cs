using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Response
{
    public class BadRequestResponse : BaseResponse
    {
        public BadRequestResponse(string message, string validationError)
        : base(message, success: false)
        {
            ValidationErrors = new List<string>
            {
                validationError
            };
        }
        public BadRequestResponse(string message, List<string> validationErrors)
            : base(message, success: false)
        {
            ValidationErrors = validationErrors;
        }
    }
}
