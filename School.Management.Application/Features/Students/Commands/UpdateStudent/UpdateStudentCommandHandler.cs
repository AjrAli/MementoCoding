using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;

namespace SchoolProject.Management.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentCommandHandler(IMapper mapper, IBaseRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateStudentCommandResponse> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var updateStudentCommandResponse = new UpdateStudentCommandResponse();
            var validator = new UpdateStudentCommandValidator();
            try
            {
                var studentToUpdate = await _studentRepository.GetAsync(request.Student.Id);
                if (studentToUpdate == null)
                    throw new NotFoundException(nameof(Student), request.Student.Id);

                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.Errors.Count > 0)
                {
                    updateStudentCommandResponse.Success = false;
                    updateStudentCommandResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors)
                    {
                        updateStudentCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                    }
                }
                if (updateStudentCommandResponse.Success)
                {

                    _mapper.Map(request.Student, studentToUpdate);
                    await _studentRepository.UpdateAsync(studentToUpdate);
                    if (await _unitOfWork.SaveChangesAsync() > 0)
                        updateStudentCommandResponse.Message = "ok";
                    else
                        updateStudentCommandResponse.Message = "Not ok";

                }
            }
            catch (Exception ex)
            {
                updateStudentCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
            return updateStudentCommandResponse;
        }
    }
}
