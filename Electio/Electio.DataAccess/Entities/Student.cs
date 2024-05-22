using Electio.DataAccess.Enums;

namespace Electio.DataAccess.Entities;
public class Student
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double AverageGrade { get; set; }

    public Specialty Specialty { get; set; }

    public Faculty Faculty { get; set; }

    public StudyYear StudyYear { get; set; }
}
