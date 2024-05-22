namespace Electio.DataAccess.Entities;
public class CourseEnrollment
{
    public string Title { get; set; } = string.Empty;

    public int Quota { get; set; }

    public IEnumerable<Student> Students { get; set; } = [];
}
