using MediatR;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Schools.Commands.DeleteSchool
{
    public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, DeleteSchoolCommandResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;

        public DeleteSchoolCommandHandler(ManagementProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteSchoolCommandResponse> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolToDelete = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id == request.SchoolId, cancellationToken);
            if (schoolToDelete == null)
            {
                throw new NotFoundException(nameof(School), request.SchoolId);
            }

            var hasStudents = await _dbContext.Students.AnyAsync(student => student.SchoolId == schoolToDelete.Id, cancellationToken);
            if (hasStudents)
            {
                return new DeleteSchoolCommandResponse
                {
                    Success = false,
                    Message = "We can't delete a school that still has students, first delete all students!"
                };
            }

            _dbContext.Schools.Remove(schoolToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteSchoolCommandResponse
            {
                Success = true,
                Message = $"School {schoolToDelete.Name} successfully deleted"
            };
        }
    }
}
