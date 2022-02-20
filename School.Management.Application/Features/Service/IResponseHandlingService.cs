using FluentValidation.Results;
using SchoolProject.Management.Application.Features.Response;
namespace SchoolProject.Management.Application.Features.Service
{
    public interface IResponseHandlingService
    {
        void ValidateRequestResult(BaseResponse baseResponse, ValidationResult validationResult);
    }
}
