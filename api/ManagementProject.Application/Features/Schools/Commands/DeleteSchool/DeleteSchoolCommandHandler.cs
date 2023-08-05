using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Schools.Commands.DeleteSchool
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
            var schoolToDelete = await _schoolRepository.GetAsync(request.SchoolId);
            if (schoolToDelete == null)
            {
                throw new NotFoundException(nameof(School), request.SchoolId);
            }

            var hasStudents = await _studentRepository.AnyAsync(student => student.SchoolId == schoolToDelete.Id);
            if (hasStudents)
            {
                return new DeleteSchoolCommandResponse
                {
                    Success = false,
                    Message = "We can't delete a school that still has students, first delete all students!"
                };
            }

            await _schoolRepository.DeleteAsync(schoolToDelete.Id);
            await _unitOfWork.SaveChangesAsync();

            return new DeleteSchoolCommandResponse
            {
                Success = true,
                Message = $"School {schoolToDelete.Name} successfully deleted"
            };
        }
    }
}
