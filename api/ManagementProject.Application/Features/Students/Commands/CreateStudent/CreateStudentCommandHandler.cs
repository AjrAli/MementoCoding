using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Schools.Commands.UpdateSchool;
using ManagementProject.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseFactory<CreateStudentCommandResponse> _responseFactory;

        public CreateStudentCommandHandler(IMapper mapper,
                                           IBaseRepository<Student> studentRepository,
                                           IUnitOfWork unitOfWork,
                                           IResponseFactory<CreateStudentCommandResponse> responseFactory)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _responseFactory = responseFactory;
        }

        public async Task<CreateStudentCommandResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var createStudentCommandResponse = _responseFactory.CreateResponse();
            await CreateStudentResponseHandling(request, createStudentCommandResponse);
            createStudentCommandResponse.Message = $"Student {request.Student?.FirstName} successfully created";
            return createStudentCommandResponse;
        }

        private async Task CreateStudentResponseHandling(CreateStudentCommand request, CreateStudentCommandResponse createStudentCommandResponse)
        {

            var student = _mapper.Map<Student>(request.Student);
            await _studentRepository.AddAsync(student);
            if (await _unitOfWork.SaveChangesAsync() <= 0)
                throw new BadRequestException($"Failed to create student : {request?.Student?.FirstName}");

        }

    }
}
