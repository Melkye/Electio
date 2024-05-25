using AutoMapper;
using Electio.BusinessLogic.DTOs;
using Electio.DataAccess;
using Electio.DataAccess.Entities;

namespace Electio.BusinessLogic.Services;
public class CoursesService
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public CoursesService(
        IMapper mapper,
        UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateRandomCoursesAsync()
    {
        if (!(await _unitOfWork.CourseRepository.GetAllAsync()).Any())
        {
            var courses = _mapper.Map<IEnumerable<Course>>(Generator.GenerateCourses());
            await _unitOfWork.CourseRepository.CreateCoursesAsync(courses);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseGetDTO>> GetAllAsync()
    {
        var courses = await _unitOfWork.CourseRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CourseGetDTO>>(courses);
    }

    public IEnumerable<CourseEnrollmentDTO> GetStudentsPerCourse()
    {
        return _mapper.Map<IEnumerable<CourseEnrollmentDTO>>(_unitOfWork.CourseRepository.GetStudentsPerCourse());
    }

    public async Task UnenrollEveryone()
    {
        await _unitOfWork.StudentOnCourseRepository.UnenrollEveryone();
        await _unitOfWork.SaveChangesAsync();

        //return await GetStudentsPerCourseAsync();
    }
}