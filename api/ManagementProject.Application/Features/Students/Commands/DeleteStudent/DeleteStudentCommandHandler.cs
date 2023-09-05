using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Contracts.MediatR.Command;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommandHandler : ICommandHandler<DeleteStudentCommand, DeleteStudentCommandResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;

        public DeleteStudentCommandHandler(ManagementProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteStudentCommandResponse> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var studentToDelete = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == request.StudentId, cancellationToken);

            if (studentToDelete == null)
            {
                throw new NotFoundException(nameof(Student), request.StudentId);
            }

            _dbContext.Students.Remove(studentToDelete);

            return new DeleteStudentCommandResponse
            {
                Message = $"Student {studentToDelete.FirstName} successfully deleted"
            };
        }
    }
}
