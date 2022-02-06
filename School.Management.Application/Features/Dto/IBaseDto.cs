using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Dto
{
    public interface IBaseDto
    {
        long Id { get; set; }
        bool Haschildren { get; set; }
        string Parentname { get; set; }
    }
}
