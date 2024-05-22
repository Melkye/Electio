
using Electio.DataAccess.Repositories;

namespace Electio.DataAccess;
public class UnitOfWork
{
    private readonly ElectioDbContext _context;

    public UnitOfWork(ElectioDbContext context)
    {
        _context = context;
        CourseRepository = new(_context);
        StudentRepository = new(_context);
        StudentOnCourseRepository = new(_context);
    }

    public CourseRepository CourseRepository { get; }

    public StudentRepository StudentRepository { get; }

    public StudentOnCourseRepository StudentOnCourseRepository { get; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
