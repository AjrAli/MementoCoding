using FluentValidation.Results;
using SchoolProject.Management.Application.Features.Response;
using System.Collections.Generic;

namespace SchoolProject.Management.Application.Features.Service
{
    public class ResponseHandlingService : IResponseHandlingService
    {
        public void ValidateRequestResult(IBaseResponse baseResponse, ValidationResult validationResult)
        {
            if (validationResult.Errors.Count > 0)
            {
                baseResponse.Success = false;
                baseResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    baseResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
        }
    }
}
