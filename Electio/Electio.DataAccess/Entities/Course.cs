using Electio.DataAccess.Enums;

// TODO: add course description, maybe picture, other stuff | don't forget to update DTOs
namespace Electio.DataAccess.Entities;
public class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Quota { get; set; }

    // TODO: decide how to store it in db
    public List<Specialty> Specialties { get; set; } = [];

    public Faculty Faculty { get; set; }

    public StudyComponent StudyComponent { get; set; }
    //public StudyYear StudyYear { get; set; }
}
