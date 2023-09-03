using AutoMapper;
using MediatR;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, UpdateSchoolCommandResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateSchoolCommandHandler(IMapper mapper, ManagementProjectDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<UpdateSchoolCommandResponse> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolToUpdate = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id == request.School.Id, cancellationToken);
            if (schoolToUpdate == null)
            {
                throw new NotFoundException(nameof(School), request?.School?.Id);
            }

            _mapper.Map(request?.School, schoolToUpdate);
            _dbContext.Schools.Update(schoolToUpdate);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateSchoolCommandResponse
            {
                Message = $"School {request?.School?.Name} successfully updated"
            };
        }
    }
}
