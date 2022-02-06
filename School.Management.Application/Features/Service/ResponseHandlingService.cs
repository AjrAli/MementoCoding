using SchoolProject.Management.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace SchoolProject.Management.Application.Features.Service
{
    public class ResponseHandlingService : IResponseHandlingService
    {
        public void ValidateRequestResult(BaseResponse baseResponse, ValidationResult validationResult)
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
