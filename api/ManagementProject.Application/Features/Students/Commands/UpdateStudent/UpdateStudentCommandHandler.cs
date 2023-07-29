using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Schools.Commands.DeleteSchool;
using ManagementProject.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;



namespace ManagementProject.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseFactory<UpdateStudentCommandResponse> _responseFactory;
        public UpdateStudentCommandHandler(IMapper mapper,
                                           IBaseRepository<Student> studentRepository,
                                           IUnitOfWork unitOfWork,
                                           IResponseFactory<UpdateStudentCommandResponse> responseFactory)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _responseFactory = responseFactory;
        }

        public async Task<UpdateStudentCommandResponse> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var updateStudentCommandResponse = _responseFactory.CreateResponse();
            await UpdateStudentResponseHandling(request, updateStudentCommandResponse);
            updateStudentCommandResponse.Message = $"Student {request.Student?.FirstName} successfully updated";
            return updateStudentCommandResponse;
        }

        private async Task UpdateStudentResponseHandling(UpdateStudentCommand request, UpdateStudentCommandResponse updateStudentCommandResponse)
        {

            var studentToUpdate = await _studentRepository.GetAsync(request?.Student?.Id);
            _mapper.Map(request?.Student, studentToUpdate);
            await _studentRepository.UpdateAsync(studentToUpdate);
            if (await _unitOfWork.SaveChangesAsync() <= 0)
                throw new BadRequestException($"Failed to update student by id : {request?.Student?.FirstName}");

        }
    }
}
