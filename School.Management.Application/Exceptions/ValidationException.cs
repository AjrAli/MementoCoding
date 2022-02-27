using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SchoolProject.Management.Application.Exceptions
{
    [Serializable]
    // Important: This attribute is NOT inherited from Exception, and MUST be specified 
    // otherwise serialization will fail with a SerializationException stating that
    // "Type X in Assembly Y is not marked as serializable."
    public class ValidationException : ApplicationException
    {

        private readonly IList<string>? _validationErrors;

        public ValidationException(ValidationResult validationResult)
        {
            _validationErrors = new List<string>();

            foreach (var validationError in validationResult.Errors)
            {
                _validationErrors.Add(validationError.ErrorMessage);
            }
        }


        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ValidationException(string message, IList<string> validationErrors)
            : base(message)
        {
            this._validationErrors = validationErrors;
        }

        public ValidationException(string message, IList<string> validationErrors, Exception innerException)
            : base(message, innerException)
        {
            _validationErrors = validationErrors;
        }


        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _validationErrors = info.GetValue("ValidationErrors", typeof(IList<string>)) as IList<string>;
        }


        public IList<string>? ValidationErrors
        {
            get { return _validationErrors; }
        }

    }
}
