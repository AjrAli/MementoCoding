using System.Collections.Generic;

namespace ManagementProject.Application.Features.Response
{
    public class BaseResponse : IBaseResponse
    {
        public BaseResponse() => Success = true;

        public BaseResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string? Message { get; set; }
        public IList<string>? ValidationErrors { get; set; }
        public long Count { get; set; }
    }
}
