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
    public async Task<IEnumerable<Course>> Get()
    {
        return await _coursesService.GetAllAsync();
    }

    // GET api/<CourseController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<CourseController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // POST api/<CourseController>
    [HttpPost("add-courses")]
    public async Task<IEnumerable<Course>> PostMany()
    {
        return await _coursesService.CreateMany();
    }

    // GET: api/<CourseController>
    [HttpGet("placement")]
    public IEnumerable<CourseEnrollmentDTO> GetEnrollment()
    {
        return _coursesService.GetStudentsPerCourse();
    }

    // GET: api/<CourseController>
    [HttpGet("unenroll-everyone")]
    public async Task UnenrollEveryone()
    {
        await _coursesService.UnenrollEveryone();

        //return await _coursesService.GetStudentsPerCourseAsync();
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
}
