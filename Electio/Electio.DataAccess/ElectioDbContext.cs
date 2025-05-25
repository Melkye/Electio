using Electio.DataAccess.Entities;
using Electio.DataAccess.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Electio.DataAccess;

public class ElectioDbContext : IdentityDbContext<ApplicationUser>
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
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();

            entity.HasOne<Student>()
                .WithOne()
                .HasForeignKey<ApplicationUser>(au => au.Name)
                .HasPrincipalKey<Student>(s => s.Name)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

                        // Configuring the one-to-one relationship without navigation properties
            entity.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Student>(e => e.Name)
                .HasPrincipalKey<ApplicationUser>(au => au.Name)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            //entity.Property(e => e.Name).IsRequired();
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

            entity.HasOne<Student>()
                .WithMany()
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasOne<Course>()
                .WithMany()
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.StudentId).IsRequired();
            entity.Property(e => e.CourseId).IsRequired();
            entity.Property(e => e.Priority).IsRequired();
            entity.Property(e => e.IsEnrolled).IsRequired();
            entity.Property(e => e.IsChecked).IsRequired();
        });
    }
}