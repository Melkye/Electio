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
}
