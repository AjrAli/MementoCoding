using System.Collections.Generic;

namespace SchoolProject.Management.Application.Features.Response
{
    public interface IBaseResponse
    {
        bool Success { get; set; }
        string? Message { get; set; }
        List<string>? ValidationErrors { get; set; }
    }
}
