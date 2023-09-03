using AutoMapper;
using MediatR;
using ManagementProject.Application.Exceptions;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ManagementProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Schools.Queries.GetSchool
{
    public class GetSchoolQueryHandler : IRequestHandler<GetSchoolQuery, GetSchoolQueryResponse>
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
