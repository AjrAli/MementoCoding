using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ManagementProject.Application.Contracts.MediatR.Query;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQueryHandler : IQueryHandler<GetSchoolQuery, GetSchoolQueryResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetSchoolQueryHandler(IMapper mapper, ManagementProjectDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetSchoolQueryResponse> Handle(GetSchoolQuery request, CancellationToken cancellationToken)
        {
            var school = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id == request.SchoolId, cancellationToken);
            if (school == null)
            {
                throw new NotFoundException(nameof(School), request?.SchoolId ?? 0);
            }

            var schoolDto = _mapper.Map<GetSchoolDto>(school);
            schoolDto.Haschildren = school.Students.Any(x => x.SchoolId == school.Id);

            return new GetSchoolQueryResponse
            {
                SchoolDto = schoolDto
            };
        }
    }
}
