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

    public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId)
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

    public async Task<IEnumerable<StudentOnCourse>> GetCoursesWithPrioritiesByStudentIdAsync(int studentId)
    {
        return await _context.StudentsOnCourses
            .Where(soc => soc.StudentId == studentId)
            .ToListAsync();
    }

    public async Task AddCoursePriorityToStudent(int studentId, int courseId, int priority)
    {
        // TODO: check if course and student exist

        var studentOnCourse = new StudentOnCourse
        {
            StudentId = studentId,
            CourseId = courseId,
            Priority = priority,
            IsEnrolled = false
        };

        _context.StudentsOnCourses.Add(studentOnCourse);

        // TODO: consider using SaveChangesAsync out of repositories
        await _context.SaveChangesAsync();
    }

    public async Task AddCoursePriorityToStudent(int studentId, string courseTitle, int priority)
    {
        // TODO: check if course and student exist
        var courseId = await _context.Courses
            .Where(c => c.Title == courseTitle)
            .Select(c => c.Id)
            .FirstOrDefaultAsync();

        AddCoursePriorityToStudent(studentId, (int)courseId, priority);
    }

    // TODO: consider returning Student but not id
    public async Task<IEnumerable<int?>> GetStudentsIdsByCourseIdAsync(int courseId)
    {
        return await _context.StudentsOnCourses
            .Where(c => c.Id == courseId)
            .Select(c => c.StudentId)
            .ToListAsync();
    }

    public async Task AddStudentToCourseAsync(int studentId, int courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student != null && course != null)
        {
            _context.StudentsOnCourses.Update(
                new StudentOnCourse
            {
                StudentId = studentId,
                CourseId = courseId,
                IsEnrolled = true
            });
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveStudentFromCourseAsync(int studentId, int courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student != null && course != null)
        {
            _context.StudentsOnCourses.Update(
                new StudentOnCourse
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    IsEnrolled = false
                });

            await _context.SaveChangesAsync();
        }
    }

    // TODO: remove this methos as it's used for algorithm only | or at least try move it to service
    public async Task MarkCourseAsChecked(int studentId, int courseId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student != null && course != null)
        {
            _context.StudentsOnCourses.Update(
                               new StudentOnCourse
                               {
                                   StudentId = studentId,
                                   CourseId = courseId,
                                   IsChecked = true
                               });

            await _context.SaveChangesAsync();
        }
    }
}
