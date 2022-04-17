using System;
using System.Runtime.Serialization;

namespace SchoolProject.Management.Application.Exceptions
{

    public class BadRequestException: Exception
    {
        private readonly string _responseException;

        public BadRequestException(string message, Exception? exception) : base(message)
        {
            _responseException = $"ERROR : {exception?.Message} {exception?.InnerException?.Source} : {exception?.InnerException?.Message}";
        }
        public string ResponseException { get { return _responseException; } }
    }
}
