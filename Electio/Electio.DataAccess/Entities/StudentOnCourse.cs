namespace Electio.DataAccess.Entities;
public class StudentOnCourse
{
    public int? Id { get; set; }

    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public int? Priority { get; set; }

    public bool IsEnrolled { get; set; } = false;

    // TODO: remove this property as it's used for algorithm and should not be stored in the database
    public bool IsChecked { get; set; } = false;
}
