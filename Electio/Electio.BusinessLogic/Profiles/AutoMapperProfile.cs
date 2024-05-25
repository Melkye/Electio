using Electio.DataAccess.Entities;
using Electio.BusinessLogic.DTOs;
using AutoMapper;

namespace Electio.BusinessLogic.Profiles;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Student, StudentGetDTO>();
        CreateMap<StudentCreateDTO, Student>();

        CreateMap<Course, CourseGetDTO>();
        CreateMap<CourseCreateDTO, Course>();

        CreateMap<CourseEnrollment, CourseEnrollmentDTO>()
            .ReverseMap();
    }   
}
