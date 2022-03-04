using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Service;
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
        private readonly ILogger<CreateStudentCommand> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandlingService _responseHandlingService;
        public CreateStudentCommandHandler(IMapper mapper, 
                                           ILogger<CreateStudentCommand> logger,
                                           IBaseRepository<Student> studentRepository,
                                           IUnitOfWork unitOfWork,
                                           IResponseHandlingService responseHandlingService)
        {
            _mapper = mapper;
            _logger = logger;
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
                _logger.LogWarning($"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}");
                createStudentCommandResponse.Message = $"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}";
                throw new BadRequestException("Create student failed!");
            }
        }

    }
}
