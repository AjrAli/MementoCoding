using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DeleteSchoolCommand> _logger;
        public DeleteSchoolCommandHandler(IBaseRepository<School> schoolRepository,
                                          ILogger<DeleteSchoolCommand> logger,
                                          IBaseRepository<Student> studentRepository,
                                          IUnitOfWork unitOfWork)
        {
            _logger = logger;
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
                long Id = (request?.SchoolId != null) ? (long)request!.SchoolId : 0;
                var schoolToDelete = await _schoolRepository.GetAsync(Id);

                if (schoolToDelete == null)
                    throw new NotFoundException(nameof(School), Id);

                if (!(_studentRepository.Any(x => x.SchoolId == schoolToDelete.Id)))
                {

                    await _schoolRepository.DeleteAsync(schoolToDelete.Id);
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
                deleteSchoolCommandResponse.Success = false;
                _logger.LogWarning($"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}");
                deleteSchoolCommandResponse.Message = $"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}";
                throw new BadRequestException("Delete school failed!");
            }
        }
    }
}
