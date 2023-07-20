using ManagementProject.Application.Features.Students.Queries.GetStudent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQuery : IRequest<GetSearchResultsQueryResponse>
    {
        public string Keyword { get; set; }
    }
}
