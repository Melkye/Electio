using Electio.BusinessLogic.DTOs;
using Electio.BusinessLogic.Services;
using Electio.DataAccess.Enums;
using Electio.DataAccess.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly StudentService _studentService;

    public UserController(UserManager<ApplicationUser> userManager, StudentService studentService)
    {
        _userManager = userManager;
        _studentService = studentService;
    }

    [HttpPost("create-student")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentModel model)
    {
        var user = new ApplicationUser { Name = model.Name, UserName = model.Username, Email = model.Email };

        var student = new StudentCreateDTO
        {
            Name = model.Name,
            AverageGrade = model.AverageGrade,
            Specialty = model.Specialty,
            Faculty = model.Faculty,
            StudyYear = model.StudyYear
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Student");
            await _studentService.CreateAsync(student);
            return Created();
        }
        return BadRequest(result.Errors);
    }
}

public class CreateStudentModel
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public double AverageGrade { get; set; }

    public Specialty Specialty { get; set; }

    public Faculty Faculty { get; set; }

    public StudyYear StudyYear { get; set; }
}
