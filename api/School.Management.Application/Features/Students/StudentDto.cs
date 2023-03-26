﻿using SchoolProject.Management.Application.Features.Dto;

namespace SchoolProject.Management.Application.Features.Students
{
    public class StudentDto : IBaseDto
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Adress { get; set; }

        // Relationships
        public long SchoolId { get; set; }
    }
}
