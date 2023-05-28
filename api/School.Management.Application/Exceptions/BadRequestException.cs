using SchoolProject.Management.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SchoolProject.Management.Application.Exceptions
{

    public class BadRequestException: Exception
    {
        private readonly string _exceptionStr;

        public BadRequestException(string message, Exception? exception) : base(message)
        {
            _exceptionStr = $"ERROR : {exception?.Message} {exception?.InnerException?.Source} : {exception?.InnerException?.Message}";
        }
        public BadRequestException(string message) : base(message)
        {
            _exceptionStr = string.Empty;
        }
        public string ExceptionStr { get { return _exceptionStr; } }

        public BadRequestResponse CreateErrorResponse()
        {
            var validationError = Message;
            return new BadRequestResponse("Invalid request.", validationError);
        }
        public BadRequestResponse CreateErrorResponse(string newErrorMessage)
        {
            var validationError = newErrorMessage;
            return new BadRequestResponse("Invalid request.", newErrorMessage);
        }
    }
}
