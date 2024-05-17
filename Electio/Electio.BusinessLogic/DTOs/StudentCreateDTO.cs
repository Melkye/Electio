namespace Electio.BusinessLogic.DTOs;
public class StudentCreateDTO
{
    public string? Name { get; set; }

    public double? AverageGrade { get; set; }

    // TODO: handle situation where not all courses are listed
    public Dictionary<string, int>? CoursesPriorities { get; set; }
}
