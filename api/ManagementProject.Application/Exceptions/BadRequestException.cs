using ManagementProject.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ManagementProject.Application.Exceptions
{

    public class BadRequestException: BaseException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
