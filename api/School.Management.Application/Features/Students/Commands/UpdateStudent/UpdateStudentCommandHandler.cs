using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Response;
using SchoolProject.Management.Application.Features.Schools.Commands.DeleteSchool;
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
            return updateStudentCommandResponse;
        }

        private async Task UpdateStudentResponseHandling(UpdateStudentCommand request, UpdateStudentCommandResponse updateStudentCommandResponse)
        {
            try
            {
                var studentToUpdate = await _studentRepository.GetAsync(request?.Student?.Id);
                if (studentToUpdate == null)
                    throw new NotFoundException(nameof(Student), request?.Student?.Id ?? 0);

                _mapper.Map(request?.Student, studentToUpdate);
                await _studentRepository.UpdateAsync(studentToUpdate);
                if (await _unitOfWork.SaveChangesAsync() <= 0)
                    updateStudentCommandResponse.Success = false;
            }
            catch (Exception ex)
            {
                var exception = new BadRequestException($"Update student failed : {ex}");
                updateStudentCommandResponse.Success = false;
                updateStudentCommandResponse.Message = exception.Message;
                throw exception;
            }
        }
    }
}
