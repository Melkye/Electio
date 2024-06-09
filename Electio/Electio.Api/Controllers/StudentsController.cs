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
        public async Task<ActionResult<StudentPrioritiesDTO>> GetStudentPriorities(Guid id)
        {
            var coursesWithPriorities = await _studentService.GetStudentPrioritiesAsync(id);

            if (coursesWithPriorities == null)
            {
                return NotFound();
            }

            return coursesWithPriorities;
        }


        [HttpGet("{id:guid}/available-courses")]
        public async Task<IDictionary<StudyComponent, List<CourseGetDTO>>> SetStudentPriorities(Guid id)
        {
            return await _studentService.GetAvailableCourses(id);
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
            return await _studentService.SetRandomPriorities(isCloseToReal: false);
        }

        [HttpPost("set-close-to-real-priorities")]
        public async Task<IEnumerable<IEnumerable<StudentOnCourse>>> SetCloseToRealPriorities()
        {
            return await _studentService.SetRandomPriorities(isCloseToReal: true);
        }

        [HttpPost("{id:guid}/priorities")]
        public async Task<IEnumerable<StudentOnCourse>> SetStudentPriorities([FromBody] StudentPrioritiesDTO dto)
        {
            return await _studentService.SetPriorities(dto);
        }

        // TODO: ActionResult?
        [HttpPost("execute-grade-biased-placement")]
        public async Task<IEnumerable<StudentGetDTO>> ExecuteGradeBiasedPlacement()
        {
            var studyComponents = Enum.GetValues<StudyComponent>();

            // TODO: check if it works
            // TODO: fix to  place all at once
            foreach (var studyComponent in studyComponents)
            {
                await _studentService.ExecuteGradeBiasedPlacement(studyComponent);
            }

            return await Get();
        }

        // TODO: ActionResult?
        [HttpPost("execute-time-biased-placement")]
        public async Task<IEnumerable<StudentGetDTO>> ExecuteTimeBiasedPlacement()
        {
            var studyComponents = Enum.GetValues<StudyComponent>();

            // TODO: check if it works
            // TODO: fix to  place all at once
            foreach (var studyComponent in studyComponents)
            {
                await _studentService.ExecuteTimeBiasedPlacement(studyComponent);
            }

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

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            await _studentService.DeleteAsync();

            return NoContent();
        }
    }
}
