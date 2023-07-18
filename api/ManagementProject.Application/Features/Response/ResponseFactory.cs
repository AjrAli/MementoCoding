using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Response
{
    public class ResponseFactory<T> : IResponseFactory<T> where T : BaseResponse, new()
    {
        public T CreateResponse()
        {
            return new T();
        }
    }
}
