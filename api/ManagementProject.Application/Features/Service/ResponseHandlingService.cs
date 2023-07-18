using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Features.Response;
using System.Collections.Generic;

namespace ManagementProject.Application.Features.Service
{
    public class ResponseHandlingService : IResponseHandlingService
    {
        public void ValidateRequestResult(ILogger logger, IBaseResponse baseResponse, ValidationResult validationResult)
        {
            if (validationResult.Errors.Count > 0)
            {
                baseResponse.Success = false;
                baseResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    logger.LogError(error.ErrorMessage);
                    baseResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
        }
    }
}
