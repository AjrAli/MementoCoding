using System.Collections.Generic;

namespace ManagementProject.Application.Features.Response
{
    public interface IBaseResponse
    {
        bool Success { get; set; }
        string? Message { get; set; }
        IList<string>? ValidationErrors { get; set; }
        long Count { get; set; }
    }
}
