using Electio.BusinessLogic.Services;
using Electio.BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Electio.DataAccess.Entities;
using Electio.DataAccess.Repositories;

namespace Electio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentGetDTO>> Get()
        {
            return await _studentService.GetAllAsync();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<StudentGetDTO>> Get(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpGet("{id:Guid}/placement")]
        public async Task<IEnumerable<CourseGetDTO>> GetStudentCourses(Guid id)
        {
            var courses = await _studentService.GetStudentCoursesAsync(id);

            if (courses == null)
            {
                return (IEnumerable<CourseGetDTO>)NotFound();
            }

            return courses;
        }

        [HttpGet("{id:Guid}/priorities")]
        public async Task<IEnumerable<StudentOnCourse>> GetStudentPriorities(Guid id)
        {
            var coursesWithPriorities = await _studentService.GetStudentPrioritiesAsync(id);

            if (coursesWithPriorities == null)
            {
                return (IEnumerable<StudentOnCourse>)NotFound();
            }

            return coursesWithPriorities;
        }

        [HttpPost]
        public async Task<ActionResult<StudentGetDTO>> Post([FromBody] StudentCreateDTO studentDTO)
        {
            var student = await _studentService.Create(studentDTO);

            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        [HttpPost("add-students")]
        public async Task<IEnumerable<StudentGetDTO>> PostStudents()
        {
            if (!(await _studentService.GetAllAsync()).Any())
            {
                await _studentService.CreateRandomStudentsAsync();
            }

            return await _studentService.GetAllAsync();
        }

        [HttpPost("set-random-priorities")]
        public async Task<IEnumerable<IEnumerable<StudentOnCourse>>> SetRandomPriorities()
        {
            return await _studentService.SetRandomPriorities();
        }

        // TODO: ActionResult?
        [HttpPost("execute-placement")]
        public async Task<IEnumerable<StudentGetDTO>> ExecutePlacementAlgorithm()
        {
            await _studentService.ExecuteGradeBasedPlacement();

            return await Get();
        }



        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, [FromBody] StudentCreateDTO studentDTO)
        //{
        //    //if (id != studentDTO.Id)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    await _studentService.UpdateStudent(student);

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _studentService.DeleteStudent(id);

        //    return NoContent();
        //}
    }
}
