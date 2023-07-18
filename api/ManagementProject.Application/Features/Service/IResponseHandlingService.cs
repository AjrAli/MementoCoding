using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Features.Response;
namespace ManagementProject.Application.Features.Service
{
    public interface IResponseHandlingService
    {
        void ValidateRequestResult(ILogger logger, IBaseResponse baseResponse, ValidationResult validationResult);
    }
}
