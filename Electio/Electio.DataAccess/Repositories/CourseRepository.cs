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

    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course> GetCourseByIdAsync(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Course> CreateCourseAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course> UpdateCourseAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task DeleteCourseAsync(int id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }   
}
