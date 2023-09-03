using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Persistence.Context;

namespace ManagementProject.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQueryHandler : IRequestHandler<GetSearchResultsQuery, GetSearchResultsQueryResponse>
    {
        private readonly ManagementProjectDbContext _dbContext;

        public GetSearchResultsQueryHandler(ManagementProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetSearchResultsQueryResponse> Handle(GetSearchResultsQuery request, CancellationToken cancellationToken)
        {
            var stringWithoutExtraSpaces = Regex.Replace(request.Keyword, @"\s{2,}", " ");
            var keywords = stringWithoutExtraSpaces.Trim().Split();
            var allSearchResults = new HashSet<GetSearchResultsDto>(new GetSearchResultsDtoComparer());

            foreach (var keyword in keywords)
            {
                // Search for schools
                var schoolResults = await SearchSchools(keyword, cancellationToken);
                allSearchResults.UnionWith(schoolResults);

                // Search for students
                var studentResults = await SearchStudents(keyword, cancellationToken);
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

            return new GetSearchResultsQueryResponse
            {
                SearchResultsDto = allSearchResults
                .OrderByDescending(x => NumberOfMatches(x, keywords))
                .ToList(),
                Count = allSearchResults
                .OrderByDescending(x => NumberOfMatches(x, keywords)).Count()
            };
        }

        private async Task<IEnumerable<GetSearchResultsDto>> SearchSchools(string keyword, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Schools
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
                .ToListAsync(cancellationToken);

            return results;
        }

        private async Task<IEnumerable<GetSearchResultsDto>> SearchStudents(string keyword, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Students
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
                .ToListAsync(cancellationToken);

            return results;
        }

        private static bool FullKeywordMatch(GetSearchResultsDto result, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (result.Title?.ToUpper().Contains(keyword.ToUpper()) == true) continue;
                if (result.Subtitle?.ToUpper().Contains(keyword.ToUpper()) == true) continue;
                if (result.Description?.ToUpper().Contains(keyword.ToUpper()) == true) continue;
                return false;
            }

            return true;
        }

        private static int NumberOfMatches(GetSearchResultsDto result, IEnumerable<string> keywords)
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
        public bool Equals(GetSearchResultsDto? x, GetSearchResultsDto? y)
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
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + (obj.Type != null ? obj.Type.GetHashCode() : 0);
                return hash;
            }
        }
    }

}
