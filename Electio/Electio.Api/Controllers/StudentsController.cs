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

        // TODO: ActionResult?
        [HttpPost("placement")]
        public async Task<IEnumerable<StudentGetDTO>> PostPlacement()
        {
            await _studentService.ExecuteGradeBasedPlacement();

            return Get().Result;
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
