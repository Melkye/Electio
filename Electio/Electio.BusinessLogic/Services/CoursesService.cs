using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Electio.BusinessLogic.DTOs;
using Electio.DataAccess;
using Electio.DataAccess.Entities;
using Electio.DataAccess.Enums;
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
            var studyYears = Enum.GetValues<StudyYear>();

            var courses = new List<Course>();

            foreach (var studyYear in studyYears)
            {
                var studyComponents = Helper.GetStudyComponentsAvailableToStudyYear(studyYear);

                var numberOfStudentsForStudyYear = (await _unitOfWork.StudentRepository.GetAllAsync())
                    .Where(s => s.StudyYear == studyYear).Count();

                foreach (var studyComponent in studyComponents)
                {
                    courses.AddRange(_mapper.Map<IEnumerable<Course>>(
                                           Generator.GenerateCoursesForStudyComponent(
                                               studyComponent: studyComponent,
                                               numberOfStudents: numberOfStudentsForStudyYear)));

                }
            }
            await _unitOfWork.CourseRepository.CreateCoursesAsync(courses);
        }
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CourseGetDTO> CreateAsync(CourseCreateDTO dto)
    {
        var course = _mapper.Map<Course>(dto);

        var createdCourse = await _unitOfWork.CourseRepository.CreateAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CourseGetDTO>(createdCourse);
    }

    public async Task<CourseGetDTO> UpdateAsync(Guid id, CourseCreateDTO dto)
    {
        var course = _mapper.Map<Course>(dto);

        var updatedCourse = await _unitOfWork.CourseRepository.UpdateAsync(id, course);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CourseGetDTO>(updatedCourse);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _unitOfWork.CourseRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseGetDTO>> GetAllAsync()
    {
        var courses = await _unitOfWork.CourseRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CourseGetDTO>>(courses);
    }

    public async Task<CourseGetDTO> GetByIdAsync(Guid id)
    {
        var course = await _unitOfWork.CourseRepository.GetByIdAsync(id);

        return _mapper.Map<CourseGetDTO>(course);
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