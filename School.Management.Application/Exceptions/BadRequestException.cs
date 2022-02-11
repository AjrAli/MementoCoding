using System;
using System.Runtime.Serialization;

namespace SchoolProject.Management.Application.Exceptions
{
    [Serializable]
    // Important: This attribute is NOT inherited from Exception, and MUST be specified 
    // otherwise serialization will fail with a SerializationException stating that
    // "Type X in Assembly Y is not marked as serializable."
    public class BadRequestException: ApplicationException
    {
        public BadRequestException(string message): base(message)
        {

        }
        // Without this constructor, deserialization will fail
        protected BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
