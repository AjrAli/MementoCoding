using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Features.Schools.Commands.UpdateSchool;
using ManagementProject.Application.Features.Students.Commands.DeleteStudent;


namespace ManagementProject.Application.Features.Schools.Commands.DeleteSchool
{
    public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, DeleteSchoolCommandResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseFactory<DeleteSchoolCommandResponse> _responseFactory;
        public DeleteSchoolCommandHandler(IBaseRepository<School> schoolRepository,
                                          IBaseRepository<Student> studentRepository,
                                          IUnitOfWork unitOfWork,
                                          IResponseFactory<DeleteSchoolCommandResponse> responseFactory)
        {
            _schoolRepository = schoolRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _responseFactory = responseFactory;
        }

        public async Task<DeleteSchoolCommandResponse> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var deleteSchoolCommandResponse = _responseFactory.CreateResponse();
            await DeleteSchoolResponseHandling(request, deleteSchoolCommandResponse);
            return deleteSchoolCommandResponse;
        }

        private async Task DeleteSchoolResponseHandling(DeleteSchoolCommand request, DeleteSchoolCommandResponse deleteSchoolCommandResponse)
        {

            long Id = request.SchoolId;
            var schoolToDelete = await _schoolRepository.GetAsync(Id);
            if (!(_studentRepository.Any(x => x.SchoolId == schoolToDelete.Id)))
            {

                await _schoolRepository.DeleteAsync(Id);
                if (await _unitOfWork.SaveChangesAsync() <= 0)
                    throw new BadRequestException($"Failed to delete school id : {Id}");
                else
                    deleteSchoolCommandResponse.Message = $"School {schoolToDelete.Name} successfully deleted";
            }
            else
            {
                deleteSchoolCommandResponse.Success = false;
                deleteSchoolCommandResponse.Message = "We can't delete a school that still have Students, first delete all students!";
            }
        }
    }
}

