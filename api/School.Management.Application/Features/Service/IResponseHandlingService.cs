using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Features.Response;
namespace SchoolProject.Management.Application.Features.Service
{
    public interface IResponseHandlingService
    {
        void ValidateRequestResult(ILogger logger, IBaseResponse baseResponse, ValidationResult validationResult);
    }
}
