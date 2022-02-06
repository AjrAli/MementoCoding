using SchoolProject.Management.Application.Features.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Management.Application.Features.Students.Queries.GetStudent
{
    public class GetStudentDto : StudentDto, IBaseDto
    {
        public long Id { get; set; }
        public bool Haschildren { get; set; }
        public string Parentname { get; set; }
    }
}
