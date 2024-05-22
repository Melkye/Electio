using System.Linq;
using Electio.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Electio.DataAccess.Repositories;
public class StudentOnCourseRepository
{
    private readonly ElectioDbContext _context;

    public StudentOnCourseRepository(ElectioDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(Guid studentId)
    {
        return await _context.Courses.Join(
            _context.StudentsOnCourses,
            course => course.Id,
            studentOnCourse => studentOnCourse.CourseId,
            (course, studentOnCourse) => new { course, studentOnCourse })
            .Where(t => t.studentOnCourse.StudentId == studentId)
            .Where(t => t.studentOnCourse.IsEnrolled == true)
            .Select(t => t.course)
            .ToListAsync();
    }

    public async Task<IEnumerable<StudentOnCourse>> GetCoursesWithPrioritiesByStudentIdAsync(Guid studentId)
    {
        return await _context.StudentsOnCourses
            .Where(soc => soc.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<StudentOnCourse> AddCoursePriorityToStudent(Guid studentId, Guid courseId, int priority)
    {
        // TODO: check if course and student exist

        var studentOnCourse = new StudentOnCourse
        {
            StudentId = studentId,
            CourseId = courseId,
            Priority = priority,
        };

        var studentOnCourseEntity = await _context.StudentsOnCourses.AddAsync(studentOnCourse);

        // TODO: consider using SaveChangesAsync out of repositories
        //await _context.SaveChangesAsync();
        return studentOnCourseEntity.Entity;
    }

    public async Task<StudentOnCourse> AddCoursePriorityToStudent(Guid studentId, string courseTitle, int priority)
    {
        // TODO: check if course and student exist
        var courseId = await _context.Courses
            .Where(c => c.Title == courseTitle)
            .Select(c => c.Id)
            .FirstOrDefaultAsync();

        return await AddCoursePriorityToStudent(studentId, courseId, priority);
    }

    // TODO: consider returning Student but not id
    public async Task<IEnumerable<Student>> GetStudentsByCourseIdAsync(Guid courseId)
    {
        var studentIds = await _context.StudentsOnCourses
        .Where(soc => soc.CourseId == courseId && soc.IsEnrolled)
        .Select(soc => soc.StudentId)
        .ToListAsync();

        return await _context.Students.Where(s => studentIds.Contains(s.Id)).ToListAsync();
    }

    public async Task<StudentOnCourse> AddStudentToCourseAsync(Guid studentId, Guid courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student != null && course != null)
        {
            var studentOnCourse = _context.StudentsOnCourses.First(soc => soc.StudentId == studentId && soc.CourseId == courseId);
            studentOnCourse.IsEnrolled = true;
            studentOnCourse.IsChecked = true;

            var studentOnCourseUpdatedEntity = _context.StudentsOnCourses
                .Update(studentOnCourse);

            //await _context.SaveChangesAsync();
            return studentOnCourseUpdatedEntity.Entity;
        }

        throw new ArgumentException("Student or course not found");
    }

    public async Task<StudentOnCourse> RemoveStudentFromCourseAsync(Guid studentId, Guid courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student != null && course != null)
        {
            var studentOnCourse = _context.StudentsOnCourses.First(soc => soc.StudentId == studentId && soc.CourseId == courseId);
            studentOnCourse.IsEnrolled = false;

            var studentOnCourseUpdatedEntity = _context.StudentsOnCourses
                .Update(studentOnCourse);

            //await _context.SaveChangesAsync();
            return studentOnCourseUpdatedEntity.Entity;
        }

        throw new ArgumentException("Student or course not found");
    }

    // TODO: remove this methos as it's used for algorithm only | or at least try move it to service
    public async Task<StudentOnCourse> MarkCourseAsChecked(Guid studentId, Guid courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student != null && course != null)
        {
            var studentOnCourse = _context.StudentsOnCourses.First(soc => soc.StudentId == studentId && soc.CourseId == courseId);
            studentOnCourse.IsChecked = true;

            var studentOnCourseUpdatedEntity = _context.StudentsOnCourses
                .Update(studentOnCourse);

            //await _context.SaveChangesAsync();
            return studentOnCourseUpdatedEntity.Entity;
        }

        throw new ArgumentException("Student or course not found");
    }

    public async Task UnenrollEveryone()
    {
        await _context.StudentsOnCourses.ForEachAsync(soc =>
        {
            soc.IsEnrolled = false;
            soc.IsChecked = false;
        });
    }
}
