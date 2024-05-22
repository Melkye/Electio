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

    public async Task<IEnumerable<Course>> CreateMany() //CourseCreateDTO dto)
    {

        if (!(await _unitOfWork.CourseRepository.GetAllAsync()).Any())
        {
            await _unitOfWork.CourseRepository.CreateCourseAsync(new Course { Id = Guid.NewGuid(), Title = "dotnet", Quota = 3 });
            await _unitOfWork.CourseRepository.CreateCourseAsync(new Course { Id = Guid.NewGuid(), Title = "node.js", Quota = 3 });
            await _unitOfWork.CourseRepository.CreateCourseAsync(new Course { Id = Guid.NewGuid(), Title = "java", Quota = 3 });
        }

        await _unitOfWork.SaveChangesAsync();

        return await _unitOfWork.CourseRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        var students = await _unitOfWork.CourseRepository.GetAllAsync();

        return await _unitOfWork.CourseRepository.GetAllAsync();
    }

    public async Task<IEnumerable<CourseEnrollmentDTO>> GetStudentsPerCourseAsync()
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