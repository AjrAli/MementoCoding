﻿using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace SchoolProject.Management.Application.Features.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, DeleteStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteStudentCommand> _logger;
        private readonly IResponseFactory<DeleteStudentCommandResponse> _responseFactory;
        public DeleteStudentCommandHandler( IBaseRepository<Student> studentRepository,
                                            IUnitOfWork unitOfWork,
                                            ILogger<DeleteStudentCommand> logger,
                                            IResponseFactory<DeleteStudentCommandResponse> responseFactory)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _responseFactory = responseFactory;
        }

        public async Task<DeleteStudentCommandResponse> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var deleteStudentCommandResponse = _responseFactory.CreateResponse();
            await DeleteStudentResponseHandling(request, deleteStudentCommandResponse);
            return deleteStudentCommandResponse;
        }

        private async Task DeleteStudentResponseHandling(DeleteStudentCommand request, DeleteStudentCommandResponse deleteStudentCommandResponse)
        {
            try
            {
                long Id = (request?.StudentId != null) ? (long)request!.StudentId : 0;
                var studentToDelete = await _studentRepository.GetAsync(Id);

                if (studentToDelete == null)
                    throw new NotFoundException(nameof(Student), Id);


                await _studentRepository.DeleteAsync(studentToDelete.Id);
                if (await _unitOfWork.SaveChangesAsync() <= 0)
                    deleteStudentCommandResponse.Success = false;
            }
            catch (Exception ex)
            {
                var exception = new BadRequestException("Delete student failed!", ex);
                deleteStudentCommandResponse.Success = false;
                deleteStudentCommandResponse.Message = exception.ResponseException;
                throw exception;
            }
        }
    }
}
