using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Students.Commands.CreateStudent
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
            return createStudentCommandResponse;
        }

        private async Task CreateStudentResponseHandling(CreateStudentCommand request, CreateStudentCommandResponse createStudentCommandResponse)
        {
            try
            {
                var student = _mapper.Map<Student>(request.Student);
                await _studentRepository.AddAsync(student);
                if (await _unitOfWork.SaveChangesAsync() <= 0)
                    createStudentCommandResponse.Success = false;
            }
            catch (Exception ex)
            {
                var exception = new BadRequestException("Create student failed!", ex);
                createStudentCommandResponse.Success = false;
                createStudentCommandResponse.Message = exception.ResponseException;
                throw exception;
            }
        }

    }
}
