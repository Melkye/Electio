using Electio.DataAccess.Entities;
using Electio.DataAccess.Enums;

namespace Electio.BusinessLogic.DTOs;
public class CourseCreateDTO
{
    public string Title { get; set; } = string.Empty;

    public int Quota { get; set; }

    public List<Specialty> Specialties { get; set; } = [];

    public Faculty Faculty { get; set; }

    public StudyComponent StudyComponent { get; set; }
}
