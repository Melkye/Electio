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

    public async Task<Course> GetByIdAsync(Guid id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        return course;
    }

    public async Task<Course> CreateAsync(Course course)
    {
        var createdCourse = await _context.Courses.AddAsync(course);
        //await _context.SaveChangesAsync();
        return createdCourse.Entity;
    }

    public async Task<Course> UpdateAsync(Guid id, Course course)
    {
        course.Id = id;
        var updatedCourse = _context.Courses.Update(course);
        //await _context.SaveChangesAsync();
        return updatedCourse.Entity;
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

    public async Task DeleteAsync(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        _context.Courses.Remove(course!);

        var studentsOnCourse = await _context.StudentsOnCourses
            .Where(soc => soc.CourseId == id)
            .ToListAsync();

        _context.StudentsOnCourses.RemoveRange(studentsOnCourse);
    }

    public async Task DeleteAsync()
    {
        //await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [StudentsOnCourses]");
        //await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Courses]");

        var studentsOnCourses = await _context.StudentsOnCourses.ToListAsync();
        _context.StudentsOnCourses.RemoveRange(studentsOnCourses);

        var courses = await _context.Courses.ToListAsync();
        _context.Courses.RemoveRange(courses);

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
        var studentIdsEnrolledOnSpecificCourse = 
            _context.StudentsOnCourses
                .Where(soc => soc.CourseId == courseId)
                .Where(soc => soc.IsEnrolled == true);

        var studentsEnrolledOnSpecificCourse=
            studentIdsEnrolledOnSpecificCourse
                .Join(_context.Students,
                    soc => soc.StudentId,
                    s => s.Id,
                    (soc, s) => s);

        var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);

        return new CourseEnrollment
        {
            Title = course!.Title,
            Quota = course!.Quota,
            Students = [.. studentsEnrolledOnSpecificCourse]
        };
    }

    public async Task<Guid> GetCourseIdByTitleAsync(string title)
    {
        return await _context.Courses
            .Where(c => c.Title == title)
            .Select(c => c.Id)
            .FirstOrDefaultAsync();
    }

    public double GetPlacementEfficiency()
    {
        var priorityGrade = _context.StudentsOnCourses
            .Where(soc => soc.IsEnrolled)
            .Join(_context.Students,
            soc => soc.StudentId,
            s => s.Id,
            (soc, s) => new { soc.Priority, s.AverageGrade });

        return priorityGrade.Sum(pg =>  pg.AverageGrade / pg.Priority)
            / priorityGrade.Sum(pg => pg.AverageGrade); 
    }
}
