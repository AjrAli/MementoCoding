using AutoMapper;
using ManagementProject.Application.Features.Students;
using ManagementProject.Application.Features.Students.Queries.GetStudent;
using ManagementProject.Application.Features.Students.Queries.GetStudents;
using ManagementProject.Domain.Entities;


#nullable disable
namespace ManagementProject.Application.Profiles.Students
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {

            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>(MemberList.None);
            CreateMap<StudentDto, Student>()
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
            CreateMap<StudentDto, GetStudentsDto>();
            CreateMap<GetStudentsDto, StudentDto>(MemberList.None);
            CreateMap<GetStudentsDto, StudentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(source => source.Age))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(source => source.SchoolId));
            CreateMap<StudentDto, GetStudentDto>();
            CreateMap<GetStudentDto, StudentDto>(MemberList.None);
            CreateMap<GetStudentDto, StudentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(source => source.Adress))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(source => source.Age))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(source => source.SchoolId));

        }
    }
}
