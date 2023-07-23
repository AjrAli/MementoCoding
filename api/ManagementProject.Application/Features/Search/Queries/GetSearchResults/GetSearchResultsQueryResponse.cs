using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Students.Queries.GetStudent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQueryResponse : BaseResponse
    {
        public List<GetSearchResultsDto>? SearchResultsDto { get; set; }
        public GetSearchResultsQueryResponse() : base()
        {

        }
    }
}
