using DotNetCore.EntityFrameworkCore;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace SchoolProject.Management.Application.Features.Schools.Commands.DeleteSchool
{
    public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, DeleteSchoolCommandResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteSchoolCommandHandler(IBaseRepository<School> schoolRepository,
                                                          IBaseRepository<Student> studentRepository,
                                                          IUnitOfWork unitOfWork)
        {
            _schoolRepository = schoolRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteSchoolCommandResponse> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var deleteSchoolCommandResponse = new DeleteSchoolCommandResponse();
            await DeleteSchoolResponseHandling(request, deleteSchoolCommandResponse);
            return deleteSchoolCommandResponse;
        }

        private async Task DeleteSchoolResponseHandling(DeleteSchoolCommand request, DeleteSchoolCommandResponse deleteSchoolCommandResponse)
        {
            try
            {
                long Id = (long)request.SchoolId;
                var schoolToDelete = await _schoolRepository.GetAsync(Id);

                if (schoolToDelete == null)
                    throw new NotFoundException(nameof(School), request.SchoolId);

                if (!(_studentRepository.Any(x => x.SchoolId == schoolToDelete.Id)))
                {

                    await _schoolRepository.DeleteAsync(schoolToDelete);
                    if (await _unitOfWork.SaveChangesAsync() <= 0)
                        deleteSchoolCommandResponse.Success = false;
                }
                else
                {
                    deleteSchoolCommandResponse.Success = false;
                    deleteSchoolCommandResponse.Message = "We can't delete a school that still have Students, first delete all students!";
                }
            }
            catch (Exception ex)
            {
                deleteSchoolCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
        }
    }
}
