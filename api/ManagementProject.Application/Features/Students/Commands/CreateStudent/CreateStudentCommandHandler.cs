﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Application.Contracts.MediatR.Command;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;

namespace ManagementProject.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand, CreateStudentCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ManagementProjectDbContext _dbContext;

        public CreateStudentCommandHandler(IMapper mapper, ManagementProjectDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CreateStudentCommandResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _mapper.Map<Student>(request.Student);
            await _dbContext.Students.AddAsync(student, cancellationToken);

            return new CreateStudentCommandResponse
            {
                Message = $"Student {request?.Student?.FirstName} successfully created"
            };
        }
    }
}
