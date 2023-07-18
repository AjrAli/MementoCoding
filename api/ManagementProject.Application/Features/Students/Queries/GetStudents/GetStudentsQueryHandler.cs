using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


#nullable disable
namespace ManagementProject.Application.Features.Students.Queries.GetStudents
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, GetStudentsQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IResponseFactory<GetStudentsQueryResponse> _responseFactory;

        public GetStudentsQueryHandler(IMapper mapper,
                                      IStudentRepository studentRepository,
                                      IResponseFactory<GetStudentsQueryResponse> responseFactory)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _responseFactory = responseFactory;
        }

        public async Task<GetStudentsQueryResponse> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var getStudentsQueryResponse = _responseFactory.CreateResponse();
            var query = _studentRepository.GetDbSetQueryable();

            if (request.Take != 0)
                query = query.OrderBy(x => x.LastName).Skip(request.Skip).Take(request.Take);

            var listStudents = query != null ? await query.Include(x => x.School).OrderBy(x => x.LastName).ToListAsync() : null;

            if (listStudents == null)
            {
                throw new NotFoundException($"No students found");
            }
            getStudentsQueryResponse.StudentsDto = _mapper.Map<List<GetStudentsDto>>(listStudents);
            getStudentsQueryResponse.Count = (await _studentRepository.CountAsync());
            return getStudentsQueryResponse;
        }
    }
}
