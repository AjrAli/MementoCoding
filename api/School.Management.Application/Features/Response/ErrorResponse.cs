using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Response
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse() { Success = false; }
        public ErrorResponse(string message)
            : base(message, success: false)
        {
        }
        public ErrorResponse(string message, string validationError)
            : base(message, success: false)
        {
            ValidationErrors = new List<string>
            {
                validationError
            };
        }
        public ErrorResponse(string message, IList<string>? validationErrors)
            : base(message, success: false)
        {
            ValidationErrors = validationErrors;
        }
    }
}
