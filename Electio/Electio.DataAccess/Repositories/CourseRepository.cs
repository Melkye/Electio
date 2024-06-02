using Electio.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Electio.DataAccess.Repositories;
public class CourseRepository
{
    private readonly ElectioDbContext _context;

    public CourseRepository(ElectioDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course> GetCourseByIdAsync(Guid id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        return course;
    }

    public async Task<Course> CreateCourseAsync(Course course)
    {
        var createdCourse = await _context.Courses.AddAsync(course);
        //await _context.SaveChangesAsync();
        return createdCourse.Entity;
    }

    public async Task CreateCoursesAsync(IEnumerable<Course> courses)
    {
        await _context.Courses.AddRangeAsync(courses);
    }

    public async Task<Course> UpdateCourseAsync(Course course)
    {
        var updatedCourse = await _context.Courses.AddAsync(course);
        //await _context.SaveChangesAsync();
        return updatedCourse.Entity;
    }

    public async Task DeleteCourseAsync(Guid id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        _context.Courses.Remove(course);
        //await _context.SaveChangesAsync();
    }

    public IEnumerable<CourseEnrollment> GetStudentsPerCourse()
    {
        var enrollment = _context.StudentsOnCourses
            .Where(soc => soc.IsEnrolled == true)
            .Join(_context.Students,
                soc => soc.StudentId,
                s => s.Id,
                (soc, s) => new { soc.CourseId, Student = s })
            .Join(_context.Courses,
                courseStudentPair => courseStudentPair.CourseId,
                course => course.Id,
                (courseStudentPair, course) => new { course.Title, course.Quota, courseStudentPair.Student })
            .GroupBy(courseStudentPair => new { courseStudentPair.Title, courseStudentPair.Quota });

        return enrollment.Select(enrollment => new CourseEnrollment
        {
            Title = enrollment.Key.Title,
            Quota = enrollment.Key.Quota,
            Students = enrollment.Select(e => e.Student)
        });
    }

    public CourseEnrollment GetStudentsPerCourse(Guid courseId)
    {
        var enrollment = _context.StudentsOnCourses
            .Where(soc => soc.CourseId == courseId)
            .Where(soc => soc.IsEnrolled == true)
            .Join(_context.Students,
                soc => soc.StudentId,
                s => s.Id,
                (soc, s) => new { soc.CourseId, Student = s })
            .Join(_context.Courses,
                courseStudentPair => courseStudentPair.CourseId,
                course => course.Id,
                (courseStudentPair, course) => new { course.Title, course.Quota, courseStudentPair.Student })
            .GroupBy(courseStudentPair => new { courseStudentPair.Title, courseStudentPair.Quota });

        return enrollment.Select(enrollment => new CourseEnrollment
        {
            Title = enrollment.Key.Title,
            Quota = enrollment.Key.Quota,
            Students = enrollment.Select(e => e.Student)
        }).Single();
    }

    public async Task<Guid> GetCourseIdByTitleAsync(string title)
    {
        return await _context.Courses
            .Where(c => c.Title == title)
            .Select(c => c.Id)
            .FirstOrDefaultAsync();
    }
}
