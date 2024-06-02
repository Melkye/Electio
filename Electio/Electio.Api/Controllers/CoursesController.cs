﻿using Electio.BusinessLogic.DTOs;
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

    // GET api/<CourseController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    // POST api/<CourseController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //}

    // POST api/<CourseController>
    [HttpPost("add-courses")]
    public async Task<IEnumerable<CourseGetDTO>> PostMany()
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

    // get id by title
    [HttpGet("get-id-by-title/{title}")]
    public async Task<Guid> GetIdByTitle(string title)
    {
        return await _coursesService.GetCourseIdByTitleAsync(title);
    }
}
