using Electio.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Electio.DataAccess.Repositories;
public class StudentRepository
{
    private readonly ElectioDbContext _context;

    public StudentRepository(ElectioDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student> GetByIdAsync(Guid id)
    {
        return await _context.Students.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        var createdStudent = await _context.Students.AddAsync(student);
        //await _context.SaveChangesAsync();
        return createdStudent.Entity;
    }

    public async Task CreateStudentsAsync(IEnumerable<Student> students)
    {
        await _context.Students.AddRangeAsync(students);
    }

    public async Task<Student> UpdateStudentAsync(Student student)
    {
        var updatedStudent = await _context.Students.AddAsync(student);
        //await _context.SaveChangesAsync();
        return updatedStudent.Entity;
    }

    public async Task DeleteStudentAsync(Guid id)
    {
        var student = await _context.Students.FirstOrDefaultAsync(c => c.Id == id);
        //_context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<CourseEnrollment> GetStudentsPerCourse()
    {
        var enrollment = _context.StudentsOnCourses
            .Where(soc => soc.IsEnrolled)
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
}
