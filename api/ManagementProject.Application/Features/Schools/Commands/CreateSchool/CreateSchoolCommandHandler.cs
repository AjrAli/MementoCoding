using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Application.Contracts.MediatR.Command;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;

namespace ManagementProject.Application.Features.Schools.Commands.CreateSchool
{
    public class CreateSchoolCommandHandler : ICommandHandler<CreateSchoolCommand, CreateSchoolCommandResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateSchoolCommandHandler(IMapper mapper, ManagementProjectDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<CreateSchoolCommandResponse> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = _mapper.Map<School>(request.School);
            await _dbContext.Schools.AddAsync(school, cancellationToken);

            return new CreateSchoolCommandResponse
            {
                Message = $"School {request.School?.Name} successfully created"
            };
        }
    }
}
