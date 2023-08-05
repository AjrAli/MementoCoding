using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, DeleteStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudentCommandHandler(IBaseRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteStudentCommandResponse> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var studentToDelete = await _studentRepository.GetAsync(request.StudentId);

            if (studentToDelete == null)
            {
                throw new NotFoundException(nameof(Student), request.StudentId);
            }

            await _studentRepository.DeleteAsync(request.StudentId);
            await _unitOfWork.SaveChangesAsync();

            return new DeleteStudentCommandResponse
            {
                Message = $"Student {studentToDelete.FirstName} successfully deleted"
            };
        }
    }
}
