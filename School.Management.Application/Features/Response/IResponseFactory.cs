using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Response
{
    public interface IResponseFactory<T> where T : BaseResponse, new()
    {
        T CreateResponse();
    }
}
