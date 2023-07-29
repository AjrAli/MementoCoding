﻿using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Schools.Commands.CreateSchool;
using ManagementProject.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace ManagementProject.Application.Features.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, DeleteStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseFactory<DeleteStudentCommandResponse> _responseFactory;
        public DeleteStudentCommandHandler(IBaseRepository<Student> studentRepository,
                                            IUnitOfWork unitOfWork,
                                            IResponseFactory<DeleteStudentCommandResponse> responseFactory)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
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

            long Id = request.StudentId;
            var studentToDelete = await _studentRepository.GetAsync(Id);
            await _studentRepository.DeleteAsync(Id);
            if (await _unitOfWork.SaveChangesAsync() <= 0)
                throw new BadRequestException($"Failed to delete student by id : {Id}");
            else
                deleteStudentCommandResponse.Message = $"Student {studentToDelete.FirstName} successfully deleted";

        }
    }
}
