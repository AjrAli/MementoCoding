using ManagementProject.Application.Features.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Search.Queries.GetSearchResults
{
    public record GetSearchResultsDto : IBaseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}
