using Electio.DataAccess.Enums;

namespace Electio.BusinessLogic.DTOs;
public class StudentCreateDTO
{
    public string Name { get; set; } = string.Empty;

    public double AverageGrade { get; set; }

    // TODO: handle situation where not all courses are listed
    // TODO: create a separate entity and mwthods for setting priorities
    //public Dictionary<string, int>? CoursesPriorities { get; set; }

    public Specialty Specialty { get; set; }

    public Faculty Faculty { get; set; }

    public StudyYear StudyYear { get; set; }
}
