using System;
using System.Runtime.Serialization;

namespace SchoolProject.Management.Application.Exceptions
{

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
        }
    }
}
