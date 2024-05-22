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

        CreateMap<CourseEnrollment, CourseEnrollmentDTO>()
            .ReverseMap();
    }   
}
