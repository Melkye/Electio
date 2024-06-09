using Electio.BusinessLogic.DTOs;
using Electio.BusinessLogic.Services;
using Electio.DataAccess.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Electio.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly CoursesService _coursesService;

    public CoursesController(CoursesService coursesService)
    {
        _coursesService = coursesService;
    }

    // GET: api/<CourseController>
    [HttpGet]
    public async Task<IEnumerable<CourseGetDTO>> Get()
    {
        return await _coursesService.GetAllAsync();
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<CourseGetDTO>> Get(Guid id)
    {
        var course = await _coursesService.GetByIdAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        return course;
    }

    // GET api/<CourseController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CourseCreateDTO dto)
    {
        var createdCourse = await _coursesService.CreateAsync(dto);

        return Created(createdCourse.Id.ToString(), createdCourse);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] CourseCreateDTO dto)
    {
        var updatedCourse = await _coursesService.UpdateAsync(id, dto);

        return Created(updatedCourse.Id.ToString(), updatedCourse);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _coursesService.DeleteAsync(id);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync()
    {
        await _coursesService.DeleteAsync();

        return NoContent();
    }

    // POST api/<CourseController>
    [HttpPost("add-random-courses")]
    public async Task<IEnumerable<CourseGetDTO>> PostManyRandomAsync()
    {
        if (!(await _coursesService.GetAllAsync()).Any())
        {
            await _coursesService.CreateRandomCoursesAsync();
        }

        return await _coursesService.GetAllAsync();
    }

    // GET: api/<CourseController>
    [HttpGet("placement")]
    public IEnumerable<CourseEnrollmentDTO> GetEnrollment()
    {
        return _coursesService.GetStudentsPerCourse();
    }

    [HttpGet("placement/{id:Guid}")]
    public CourseEnrollmentDTO GetEnrollmentForCourse(Guid id)
    {
        return _coursesService.GetStudentsPerCourse(id);
    }

    // GET: api/<CourseController>
    [HttpGet("unenroll-everyone")]
    public async Task UnenrollEveryone()
    {
        await _coursesService.UnenrollEveryone();

        //return await _coursesService.GetStudentsPerCourseAsync();
    }

    [HttpGet("placement-status")]
    public bool GetPlacementStatus()
    {
        return _coursesService.GetStudentsPerCourse().Any();
    }

    //// PUT api/<CourseController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<CourseController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}

    [HttpGet("get-id-by-title/{title}")]
    public async Task<Guid> GetIdByTitle(string title)
    {
        return await _coursesService.GetCourseIdByTitleAsync(title);
    }

    [HttpGet("placement-efficiency")]
    public double GetPlacementEfficiency()
    {
        return _coursesService.GetPlacementEfficiency();
    }



    [HttpDelete("placement")]
    public async Task<IActionResult> DeletePlacementsAsync()
    {
        await _coursesService.DeletePlacementsAsync();

        return NoContent();
    }
}
