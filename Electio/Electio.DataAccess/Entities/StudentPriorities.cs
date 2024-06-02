namespace Electio.DataAccess.Entities;
public class StudentPriorities
{
    public string StudentName { get; set; } = string.Empty;

    public Dictionary<StudyComponent, Dictionary<string, int>> CoursesPriorities { get; set; } = [];
}

//public class StudentPriorities
//{
//    public string StudentName { get; set; } = string.Empty;

//    public List<ComponentPriorities> ComponentsPriorities { get; set; } = [];
//}

//public class ComponentPriorities
//{
//    public StudyComponent StudyComponent { get; set; }

//    public Dictionary<string, int> Priorities { get; set; } = [];
//}
