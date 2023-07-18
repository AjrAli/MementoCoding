using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ManagementProject.Application.Exceptions
{
    [Serializable]
    public class ValidationException : BaseException
    {

        private readonly IList<string>? _validationErrors;

        public ValidationException(ValidationResult validationResult) : base("Erreur de validations de champs")
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
        protected ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
            _validationErrors = info.GetValue("ValidationErrors", typeof(IList<string>)) as IList<string>;
        }

    }
}
