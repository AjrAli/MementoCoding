using AutoMapper;
using SchoolProject.Management.Application.Features.Schools.Commands.CreateSchool;
using SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchool;
using SchoolProject.Management.Application.Features.Schools.Queries.GetSchools;
using SchoolProject.Management.Domain.Entities;

namespace SchoolProject.Management.Application.Profiles.Schools
{
    public class SchoolMappingProfile : Profile
    {
        public SchoolMappingProfile()
        {

            // School profile
            CreateMap<School, CreateSchoolDto>().ReverseMap();
            CreateMap<School, UpdateSchoolDto>();
            CreateMap<UpdateSchoolDto, School>(MemberList.None);
            CreateMap<UpdateSchoolDto, School>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.Town));
            CreateMap<School, GetSchoolDto>().ReverseMap();
            CreateMap<School, GetSchoolsDto>().ReverseMap();
        }
    }
}
