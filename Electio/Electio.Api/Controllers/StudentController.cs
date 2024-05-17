using Electio.BusinessLogic.Services;
using Electio.BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Electio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentGetDTO>> Get()
        {
            return await _studentService.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentGetDTO>> Get(int id)
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

        // TODO: ActionResult?
        [HttpPost("placement")]
        public async Task<IEnumerable<StudentGetDTO>> Post()
        {
            {
                await Post(new()
                {
                    AverageGrade = 98,
                    CoursesPriorities =
                        new Dictionary<string, int>()
                            {
                            { "dotnet", 1 },
                            { "node.js", 2 },
                            { "java", 3 }
                            }
                });

                await Post(new()
                {
                    AverageGrade = 97,
                    CoursesPriorities =
                        new Dictionary<string, int>()
                        {
                        { "node.js", 1 },
                        { "dotnet", 2 },
                        { "java", 3 }
                        }
                });


                await Post(new()
                {
                    AverageGrade = 96,
                    CoursesPriorities =
                        new Dictionary<string, int>()
                        {
            { "java", 1 },
            { "dotnet", 2 },
            { "node.js", 3 }
                        }
                });


                await Post(new()
                {
                    AverageGrade = 95,
                    CoursesPriorities =
                        new Dictionary<string, int>()
                        {
            { "java", 1 },
            { "dotnet", 2 },
            { "node.js", 3 }
                        }
                });


                await Post(new()
                {
                    AverageGrade = 94,
                    CoursesPriorities =
            new Dictionary<string, int>()
            {
            { "dotnet", 1 },
            { "java", 2 },
            { "node.js", 3 }
            }
                });

                await Post(new()
                {
                    AverageGrade = 93,
                    CoursesPriorities =
            new Dictionary<string, int>()
            {
            { "node.js", 1 },
            { "java", 2 },
            { "dotnet", 3 }
            }
                });

                await Post(new()
                {
                    AverageGrade = 92,
                    CoursesPriorities =
            new Dictionary<string, int>()
            {
            { "dotnet", 1 },
            { "java", 2 },
            { "node.js", 3 }
            }
                });

                await Post(new()
                {
                    AverageGrade = 92,
                    CoursesPriorities =
            new Dictionary<string, int>()
            {
            { "dotnet", 1 },
            { "java", 2 },
            { "node.js", 3 }
            }
                });

                await Post(new()
                {
                    AverageGrade = 90,
                    CoursesPriorities =
            new Dictionary<string, int>()
            {
            { "dotnet", 1 },
            { "node.js", 2 },
            { "java", 3 }
            }
                });
            }

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
