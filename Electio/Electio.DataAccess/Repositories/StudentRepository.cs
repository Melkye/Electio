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

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        return await _context.Students.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student> UpdateStudentAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task DeleteStudentAsync(int id)
    {
        var student = await _context.Students.FirstOrDefaultAsync(c => c.Id == id);
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }
}
