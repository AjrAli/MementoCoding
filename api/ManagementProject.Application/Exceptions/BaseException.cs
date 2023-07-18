using ManagementProject.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Application.Exceptions
{
    public abstract class BaseException : Exception
    {
        private readonly IList<string>? _validationErrors;
        protected IList<string>? ValidationErrors { get { return _validationErrors; } }
        protected BaseException(string message) : base(message)
        {
        }
        protected BaseException(string message, IList<string> validationErrors) : base(message)
        {
            _validationErrors = validationErrors;
        }
        protected BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _validationErrors = info.GetValue("ValidationErrors", typeof(IList<string>)) as IList<string>;
        }
        public ErrorResponse CreateErrorResponse(string? newValidationErrorStr = null)
        {
            if (newValidationErrorStr != null)
            {
                return new ErrorResponse(Message, newValidationErrorStr);
            }
            return new ErrorResponse(Message, ValidationErrors);
        }
    }
}
