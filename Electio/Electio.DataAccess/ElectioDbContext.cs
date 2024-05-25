using Electio.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Electio.DataAccess;

public class ElectioDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentOnCourse> StudentsOnCourses { get; set; }

    public ElectioDbContext(DbContextOptions<ElectioDbContext> options) : base(options)
    {

    }


    // TODO: add OnDeleteCascade
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.AverageGrade).IsRequired();
            entity.Property(e => e.Specialty).HasConversion<string>().IsRequired();
            entity.Property(e => e.Faculty).HasConversion<string>().IsRequired();
            entity.Property(e => e.StudyYear).HasConversion<string>().IsRequired();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Quota).IsRequired();
        });

        modelBuilder.Entity<StudentOnCourse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.StudentId).IsRequired();
            entity.Property(e => e.CourseId).IsRequired();
            entity.Property(e => e.Priority).IsRequired();
            entity.Property(e => e.IsEnrolled).IsRequired();
            entity.Property(e => e.IsChecked).IsRequired();
        });
    }
}