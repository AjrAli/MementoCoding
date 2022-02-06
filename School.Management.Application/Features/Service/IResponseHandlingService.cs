using SchoolProject.Management.Application.Features.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
namespace SchoolProject.Management.Application.Features.Service
{
    public interface IResponseHandlingService
    {
        void ValidateRequestResult(BaseResponse baseResponse, ValidationResult validationResult);
    }
}
