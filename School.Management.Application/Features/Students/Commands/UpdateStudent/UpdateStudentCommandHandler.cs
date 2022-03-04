﻿using AutoMapper;
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



namespace SchoolProject.Management.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandlingService _responseHandlingService;
        private readonly ILogger<UpdateStudentCommand> _logger;
        public UpdateStudentCommandHandler(IMapper mapper,
                                           ILogger<UpdateStudentCommand> logger,
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

        public async Task<UpdateStudentCommandResponse> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var updateStudentCommandResponse = new UpdateStudentCommandResponse();
            var validator = new UpdateStudentCommandValidator();
            await UpdateStudentResponseHandling(request, updateStudentCommandResponse, validator);
            return updateStudentCommandResponse;
        }

        private async Task UpdateStudentResponseHandling(UpdateStudentCommand request, UpdateStudentCommandResponse updateStudentCommandResponse, UpdateStudentCommandValidator validator)
        {
            try
            {
                var studentToUpdate = await _studentRepository.GetAsync(request?.Student?.Id);
                if (studentToUpdate == null)
                    throw new NotFoundException(nameof(Student), request?.Student?.Id ?? 0);

                var validationResult = await validator.ValidateAsync(request);
                _responseHandlingService.ValidateRequestResult(updateStudentCommandResponse, validationResult);
                if (updateStudentCommandResponse.Success)
                {
                    _mapper.Map(request?.Student, studentToUpdate);
                    await _studentRepository.UpdateAsync(studentToUpdate);
                    if (await _unitOfWork.SaveChangesAsync() <= 0)
                        updateStudentCommandResponse.Success = false;
                }
            }
            catch (Exception ex)
            {
                updateStudentCommandResponse.Success = false;
                _logger.LogWarning($"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}");
                updateStudentCommandResponse.Message = $"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}";
                throw new BadRequestException("Update student failed!");
            }
        }
    }
}
