using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Application.Contracts.MediatR.Command;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand, UpdateStudentCommandResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateStudentCommandHandler(IMapper mapper, ManagementProjectDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<UpdateStudentCommandResponse> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var studentToUpdate = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == request.Student.Id, cancellationToken);

            if (studentToUpdate == null)
            {
                throw new NotFoundException(nameof(Student), request?.Student?.Id);
            }

            _mapper.Map(request?.Student, studentToUpdate);
            _dbContext.Students.Update(studentToUpdate);

            return new UpdateStudentCommandResponse
            {
                Message = $"Student {request?.Student?.FirstName} successfully updated"
            };
        }
    }
}
