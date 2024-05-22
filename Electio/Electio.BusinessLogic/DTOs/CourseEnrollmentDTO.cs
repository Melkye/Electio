namespace Electio.BusinessLogic.DTOs;
public class CourseEnrollmentDTO
{
    public string Title { get; set; } = string.Empty;
    
    public IEnumerable<StudentGetDTO> Students { get; set; } = [];
}
