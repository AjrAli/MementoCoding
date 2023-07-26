using AutoMapper;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Features.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
            var allSearchResults = new HashSet<GetSearchResultsDto>(new GetSearchResultsDtoComparer());

            foreach (var keyword in keywords)
            {
                // Search for schools
                var schoolResults = await SearchSchools(keyword);
                allSearchResults.UnionWith(schoolResults);

                // Search for students
                var studentResults = await SearchStudents(keyword);
                allSearchResults.UnionWith(studentResults);
            }

            // Filter results if multiple keywords were provided
            if (keywords.Length > 1)
            {
                allSearchResults = new HashSet<GetSearchResultsDto>(allSearchResults
                    .Where(r => FullKeywordMatch(r, keywords)),
                    new GetSearchResultsDtoComparer());
            }

            // Order results by the number of keyword matches
            getStudentsQueryResponse.SearchResultsDto = allSearchResults
                .OrderByDescending(x => NumberOfMatches(x, keywords))
                .ToList();

            getStudentsQueryResponse.Count = getStudentsQueryResponse.SearchResultsDto.Count;

            return getStudentsQueryResponse;
        }

        private async Task<IEnumerable<GetSearchResultsDto>> SearchSchools(string keyword)
        {
            var results = await _schoolRepository.Queryable
                .Where(x => x.Id.ToString().Contains(keyword) ||
                            x.Name.Contains(keyword) ||
                            x.Adress.Contains(keyword) ||
                            x.Town.Contains(keyword) ||
                            x.Description.Contains(keyword))
                .Select(x => new GetSearchResultsDto
                {
                    Id = x.Id,
                    Type = "schools",
                    Title = x.Name,
                    Subtitle = $"{x.Adress} - {x.Town}",
                    Description = x.Description
                })
                .ToListAsync();

            return results;
        }

        private async Task<IEnumerable<GetSearchResultsDto>> SearchStudents(string keyword)
        {
            var results = await _studentRepository.Queryable
                .Where(x => x.Id.ToString().Contains(keyword) ||
                            x.FirstName.Contains(keyword) ||
                            x.LastName.Contains(keyword) ||
                            x.Age.ToString().Contains(keyword) ||
                            x.Adress.Contains(keyword))
                .Select(x => new GetSearchResultsDto
                {
                    Id = x.Id,
                    Type = "students",
                    Title = x.FirstName,
                    Subtitle = x.LastName,
                    Description = $"{x.Age} - {x.Adress}"
                })
                .ToListAsync();

            return results;
        }

        private bool FullKeywordMatch(GetSearchResultsDto result, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (result.Title?.ToUpper().Contains(keyword) == true) continue;
                if (result.Subtitle?.ToUpper().Contains(keyword) == true) continue;
                if (result.Description?.ToUpper().Contains(keyword) == true) continue;

                return false;
            }

            return true;
        }

        private int NumberOfMatches(GetSearchResultsDto result, IEnumerable<string> keywords)
        {
            var counter = 0;

            foreach (var keyword in keywords)
            {
                var match = false;

                if (result.Title?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Subtitle?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Description?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;

                if (match)
                    counter++;
            }

            return counter;
        }
    }

    // Custom comparer to handle duplicates in the list
    public class GetSearchResultsDtoComparer : IEqualityComparer<GetSearchResultsDto>
    {
        public bool Equals(GetSearchResultsDto x, GetSearchResultsDto y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Type == y.Type;
        }

        public int GetHashCode(GetSearchResultsDto obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (obj.Id != null ? obj.Id.GetHashCode() : 0);
                hash = hash * 23 + (obj.Type != null ? obj.Type.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
