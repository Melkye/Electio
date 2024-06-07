namespace Electio.BusinessLogic.DTOs;
public class CourseEnrollmentDTO
{
    // TODO: return not just the title but the whole course
    // TODO: check placement status by cheking if course's quota is filfilled (assigned studs == all studs)
    public string Title { get; set; } = string.Empty;
    
    public IEnumerable<StudentGetDTO> Students { get; set; } = [];
}
