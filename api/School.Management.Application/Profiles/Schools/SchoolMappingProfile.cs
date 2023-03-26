using AutoMapper;
using SchoolProject.Management.Application.Features.Schools;
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
            CreateMap<School, SchoolDto>();
            CreateMap<SchoolDto, School>(MemberList.None);
            CreateMap<SchoolDto, School>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.Town));

            CreateMap<School, GetSchoolDto>();
            CreateMap<GetSchoolDto, School>(MemberList.None);
            CreateMap<GetSchoolDto, School>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.Town));
            CreateMap<School, GetSchoolsDto>();
            CreateMap<GetSchoolsDto, School>(MemberList.None);
            CreateMap<GetSchoolsDto, School>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.Town));


            CreateMap<SchoolDto, GetSchoolsDto>();
            CreateMap<GetSchoolsDto, SchoolDto>(MemberList.None);
            CreateMap<GetSchoolsDto, SchoolDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.Town));
            CreateMap<SchoolDto, GetSchoolDto>();
            CreateMap<GetSchoolDto, SchoolDto>(MemberList.None);
            CreateMap<GetSchoolDto, SchoolDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.Town));
        }
    }
}
