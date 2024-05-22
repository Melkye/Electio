namespace Electio.DataAccess.Entities;
public class StudentOnCourse
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public int Priority { get; set; }

    public bool IsEnrolled { get; set; } = false;

    // TODO: remove this property as it's used for algorithm and should not be stored in the database
    public bool IsChecked { get; set; } = false;
}
