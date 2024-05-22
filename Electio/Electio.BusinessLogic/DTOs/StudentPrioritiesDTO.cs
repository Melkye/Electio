using Electio.DataAccess.Entities;

namespace Electio.BusinessLogic.DTOs;
public class StudentPrioritiesDTO
{
    public string StudentName { get; set; } = string.Empty;

    public Dictionary<StudyComponent, Dictionary<string, int>> CoursesPriorities { get; set; } =  [];
}
