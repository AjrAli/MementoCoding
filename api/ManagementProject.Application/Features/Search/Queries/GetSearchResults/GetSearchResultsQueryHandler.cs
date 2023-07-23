using AutoMapper;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Features.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQueryHandler : IRequestHandler<GetSearchResultsQuery, GetSearchResultsQueryResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;
        private readonly IResponseFactory<GetSearchResultsQueryResponse> _responseFactory;

        public GetSearchResultsQueryHandler(IMapper mapper,
                                      IStudentRepository studentRepository,
                                      ISchoolRepository schoolRepository,
                                      IResponseFactory<GetSearchResultsQueryResponse> responseFactory)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _schoolRepository = schoolRepository;
            _responseFactory = responseFactory;
        }

        public async Task<GetSearchResultsQueryResponse> Handle(GetSearchResultsQuery request, CancellationToken cancellationToken)
        {
            var getStudentsQueryResponse = _responseFactory.CreateResponse();
            var keywords = request.Keyword.ToUpper().Trim().Split();
            getStudentsQueryResponse.SearchResultsDto = new List<GetSearchResultsDto>();
            foreach (var keyword in keywords)
            {
                // Search for schools
                getStudentsQueryResponse.SearchResultsDto.AddRange(await SearchSchools(_schoolRepository, keyword));
                // Search for students
                getStudentsQueryResponse.SearchResultsDto.AddRange(await SearchStudents(_studentRepository, keyword));
            }

            if (keywords.Length > 1)
            {
                getStudentsQueryResponse.SearchResultsDto = getStudentsQueryResponse.SearchResultsDto
                                                // All keywords should match
                                                .Where(r => FullKeywordMatch(r, keywords))
                                                .ToList();

            }

            if (!getStudentsQueryResponse.SearchResultsDto.Any())
            {
                return getStudentsQueryResponse;
            }

            // First show results with highest amount of matches
            getStudentsQueryResponse.SearchResultsDto = getStudentsQueryResponse.SearchResultsDto
                                            .OrderByDescending(x => NumberOfMatches(x, keywords))
                                            .ToList();
            getStudentsQueryResponse.Count = getStudentsQueryResponse.SearchResultsDto.Count;

            return getStudentsQueryResponse;
        }

        private static async Task<List<GetSearchResultsDto>> SearchSchools(ISchoolRepository schoolRepository, string keyword)
        {



            var results = await schoolRepository.Queryable
                                                .Where(x => x.Name.Contains(keyword) ||
                                                            x.Adress.Contains(keyword) ||
                                                            x.Town.Contains(keyword) ||
                                                            x.Description.Contains(keyword))
                                                .ToListAsync();

            return results.Select(x => new GetSearchResultsDto
            {
                Id = x.Id,
                Type = "school",
                Title = x.Name,
                Subtitle = $"{x.Adress} - {x.Town}",
                Description = x.Description
            }).ToList();
        }
        private static async Task<List<GetSearchResultsDto>> SearchStudents(IStudentRepository studentRepository, string keyword)
        {
            var results = await studentRepository.Queryable
                                                 .Where(x => x.FirstName.Contains(keyword) ||
                                                             x.LastName.Contains(keyword) ||
                                                             x.Age.ToString().Contains(keyword) ||
                                                             x.Adress.Contains(keyword))
                                                 .ToListAsync();

            return results.Select(x => new GetSearchResultsDto
            {
                Id = x.Id,
                Type = "student",
                Title = x.FirstName,
                Subtitle = x.LastName,
                Description = $"{x.Age} - {x.Adress}"
            }).ToList();
        }

        private bool FullKeywordMatch(GetSearchResultsDto result, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (result.Title?.Contains(keyword) == true) continue;
                if (result.Subtitle?.ToUpper().Contains(keyword) == true) continue;
                if (result.Description?.ToUpper().Contains(keyword) == true) continue;

                return false;
            }

            return true;
        }

        private static int NumberOfMatches(GetSearchResultsDto result, IEnumerable<string> keywords)
        {
            var counter = 0;

            foreach (var keyword in keywords)
            {
                if (result.Title?.ToUpper().Contains(keyword) == true)
                    counter++;
                if (result.Subtitle?.ToUpper().Contains(keyword) == true)
                    counter++;
                if (result.Description?.ToUpper().Contains(keyword) == true)
                    counter++;
            }
            return counter;
        }

    }
}
