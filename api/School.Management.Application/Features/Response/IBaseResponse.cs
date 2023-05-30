using System.Collections.Generic;

namespace SchoolProject.Management.Application.Features.Response
{
    public interface IBaseResponse
    {
        bool Success { get; set; }
        string? Message { get; set; }
        IList<string>? ValidationErrors { get; set; }
    }
}
