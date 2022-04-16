using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Response
{
    public class ResponseFactory<T> : IResponseFactory<T> where T : class, new()
    {
        public T CreateResponse()
        {
            return new T();
        }
    }
}
