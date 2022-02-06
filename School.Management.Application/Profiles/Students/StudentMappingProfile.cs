using AutoMapper;
using SchoolProject.Management.Application.Features.Students.Commands.CreateStudent;
using SchoolProject.Management.Application.Features.Students.Commands.UpdateStudent;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudent;
using SchoolProject.Management.Application.Features.Students.Queries.GetStudents;
using SchoolProject.Management.Domain.Entities;

namespace SchoolProject.Management.Application.Profiles.Students
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {

            CreateMap<Student, CreateStudentDto>().ReverseMap();
            CreateMap<Student, UpdateStudentDto>();
            CreateMap<UpdateStudentDto, Student>(MemberList.None);
            CreateMap<UpdateStudentDto, Student>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(source => source.Age))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(source => source.SchoolId));
            CreateMap<Student, GetStudentDto>(MemberList.None);
            CreateMap<Student, GetStudentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(source => source.Age))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(source => source.SchoolId))
                .ForMember(dest => dest.Parentname, opt => opt.MapFrom(source => source.School.Name));
            CreateMap<GetStudentDto, Student>(MemberList.None);
            CreateMap<GetStudentDto, Student>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(source => source.Age))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(source => source.SchoolId));

            CreateMap<Student, GetStudentsDto>(MemberList.None);
            CreateMap<Student, GetStudentsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(source => source.Age))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(source => source.SchoolId))
                .ForMember(dest => dest.Parentname, opt => opt.MapFrom(source => source.School.Name));
            CreateMap<GetStudentsDto, Student>(MemberList.None);
            CreateMap<GetStudentsDto, Student>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(source => source.Age))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(source => source.SchoolId));

        }
    }
}
