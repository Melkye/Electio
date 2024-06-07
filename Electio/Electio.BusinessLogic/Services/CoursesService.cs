using AutoMapper;
using Electio.BusinessLogic.DTOs;
using Electio.DataAccess;
using Electio.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

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
            var coursesSK1 = _mapper.Map<IEnumerable<Course>>(
                Generator.GenerateCoursesForStudyComponent(studyComponent: StudyComponent.SK1));

            var coursesSK2 = _mapper.Map<IEnumerable<Course>>(
                Generator.GenerateCoursesForStudyComponent(studyComponent: StudyComponent.SK2));

            var coursesSK3 = _mapper.Map<IEnumerable<Course>>(
                Generator.GenerateCoursesForStudyComponent(studyComponent: StudyComponent.SK3));

            var coursesSK4 = _mapper.Map<IEnumerable<Course>>(
                Generator.GenerateCoursesForStudyComponent(studyComponent: StudyComponent.SK4));

            await _unitOfWork.CourseRepository.CreateCoursesAsync(
                coursesSK1.Union(coursesSK2).Union(coursesSK3).Union(coursesSK4));
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

    public CourseEnrollmentDTO GetStudentsPerCourse(Guid courseId)
    {
        return _mapper.Map<CourseEnrollmentDTO>(_unitOfWork.CourseRepository.GetStudentsPerCourse(courseId));
    }

    public async Task UnenrollEveryone()
    {
        await _unitOfWork.StudentOnCourseRepository.UnenrollEveryone();
        await _unitOfWork.SaveChangesAsync();

        //return await GetStudentsPerCourseAsync();
    }

    public async Task DeletePlacementsAsync()
    {
        await _unitOfWork.StudentOnCourseRepository.DeleteAsync();
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Guid> GetCourseIdByTitleAsync(string title)
    {
        var courseId = await _unitOfWork.CourseRepository.GetCourseIdByTitleAsync(title);

        return courseId;
    }

    public double GetPlacementEfficiency()
    {
        return _unitOfWork.CourseRepository.GetPlacementEfficiency();
    }

    public async Task DeleteAsync()
    {
        await _unitOfWork.CourseRepository.DeleteAsync();
        await _unitOfWork.SaveChangesAsync();
    }
}