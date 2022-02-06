using AutoMapper;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;
using System;
using FluentValidation.Results;
using SchoolProject.Management.Application.Features.Service;

namespace SchoolProject.Management.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandlingService _responseHandlingService;
        public CreateStudentCommandHandler(IMapper mapper, IBaseRepository<Student> studentRepository, IUnitOfWork unitOfWork, IResponseHandlingService responseHandlingService)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _responseHandlingService = responseHandlingService;
        }

        public async Task<CreateStudentCommandResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var createStudentCommandResponse = new CreateStudentCommandResponse();
            var validator = new CreateStudentCommandValidator();
            await CreateStudentResponseHandling(request, createStudentCommandResponse, validator);
            return createStudentCommandResponse;
        }

        private async Task CreateStudentResponseHandling(CreateStudentCommand request, CreateStudentCommandResponse createStudentCommandResponse, CreateStudentCommandValidator validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                _responseHandlingService.ValidateRequestResult(createStudentCommandResponse, validationResult);
                if (createStudentCommandResponse.Success)
                {
                    var student = _mapper.Map<Student>(request.Student);
                    await _studentRepository.AddAsync(student);
                    if (await _unitOfWork.SaveChangesAsync() <= 0)
                        createStudentCommandResponse.Success = false;
                }
            }
            catch (Exception ex)
            {
                createStudentCommandResponse.Success = false;
                createStudentCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
        }

    }
}
