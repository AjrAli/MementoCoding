using AutoMapper;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;
using System;

namespace SchoolProject.Management.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(IMapper mapper, IBaseRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateStudentCommandResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var createStudentCommandResponse = new CreateStudentCommandResponse();
            var validator = new CreateStudentCommandValidator();
            await ResponseHandling(request, createStudentCommandResponse, validator);
            return createStudentCommandResponse;
        }

        private async Task ResponseHandling(CreateStudentCommand request, CreateStudentCommandResponse createStudentCommandResponse, CreateStudentCommandValidator validator)
        {
            try
            {
                await ValidateRequest(request, createStudentCommandResponse, validator);
                if (createStudentCommandResponse.Success)
                {


                    var student = _mapper.Map<Student>(request.Student);
                    await _studentRepository.AddAsync(student);
                    if (await _unitOfWork.SaveChangesAsync() > 0)
                        createStudentCommandResponse.Message = "ok";
                    else
                        createStudentCommandResponse.Message = "Not ok";
                }
            }
            catch (Exception ex)
            {
                createStudentCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
        }

        private static async Task ValidateRequest(CreateStudentCommand request, CreateStudentCommandResponse createStudentCommandResponse, CreateStudentCommandValidator validator)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                createStudentCommandResponse.Success = false;
                createStudentCommandResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createStudentCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
        }
    }
}
