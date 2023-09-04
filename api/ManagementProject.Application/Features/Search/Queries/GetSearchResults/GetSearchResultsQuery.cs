using ManagementProject.Application.Contracts.MediatR.Query;

namespace ManagementProject.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQuery : IQuery<GetSearchResultsQueryResponse>
    {
        public string Keyword { get; set; }
    }
}
