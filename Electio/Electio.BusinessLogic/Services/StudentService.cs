using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Electio.BusinessLogic.DTOs;
using Electio.DataAccess.Entities;
using Electio.DataAccess.Repositories;

namespace Electio.BusinessLogic.Services;
public class StudentService
{
    private readonly IMapper _mapper;
    //private readonly IUnitOfWork _unitOfWork;
    private readonly StudentRepository _studentRepository;
    private readonly StudentOnCourseRepository _studentOnCourseRepository;

    // TODO: add courses service and relete repo from here
    private readonly CourseRepository _courseRepository;

    public StudentService(
        IMapper mapper,
        StudentRepository studentRepository,
        StudentOnCourseRepository studentOnCourseRepository,
        CourseRepository courseRepository)
    {
        _mapper = mapper;
        _studentRepository = studentRepository;
        _studentOnCourseRepository = studentOnCourseRepository;
        _courseRepository = courseRepository;
    }

    public async Task<StudentGetDTO> Create(StudentCreateDTO dto)
    {
        // TODO: add mapping
        Student student = new() { Name = dto.Name, AverageGrade = dto.AverageGrade };

        student = await _studentRepository.CreateStudentAsync(student);

        foreach (var coursePriority in dto.CoursesPriorities!)
        {
            await _studentOnCourseRepository.AddCoursePriorityToStudent((int)student.Id!, coursePriority.Key, coursePriority.Value);
        };

        return _mapper.Map<StudentGetDTO>(student);
    }

    public async Task<IEnumerable<StudentGetDTO>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllStudentsAsync();

        return _mapper.Map<IEnumerable<StudentGetDTO>>(students);
    }

    public async Task<StudentGetDTO> GetByIdAsync(int id)
    {
        var student = await _studentRepository.GetStudentByIdAsync(id);

        return _mapper.Map<StudentGetDTO>(student);
    }

    // TODO: consider moving it to StudentOnCourseService
    public async Task ExecuteGradeBasedPlacement()
    {
        // NOTE: courses in Student should be ordered from highest(1) to lowest(len) priority
        // NOTE: consider ordering from lowest to highest to remove at last pos
        //public List<Course> GradeBasedPlacement(List<Student> students)

        // TODO: add titles and quotas here
        await _courseRepository.CreateCourseAsync(new Course { Title = "dotnet", Quota = 3 });
        await _courseRepository.CreateCourseAsync(new Course { Title = "node.js", Quota = 3 });
        await _courseRepository.CreateCourseAsync(new Course { Title = "java", Quota = 3 });

        // TODO: get rid of this variable
        var unassignedStudents = new List<Student>(await _studentRepository.GetAllStudentsAsync());

        while (unassignedStudents.Any())
        {
            var currentStudent = unassignedStudents.Last();

            var highestPriorityNotCheckedCourseId = _studentOnCourseRepository.GetCoursesWithPrioritiesByStudentIdAsync((int)currentStudent.Id!).Result
                .Where(soc => soc.IsChecked == false)
                .OrderBy(soc => soc.Priority).First().Id;

            //var course = placement.First(course => course.Title == highestPriorityCourseId);

            if (_studentOnCourseRepository.GetStudentsIdsByCourseIdAsync((int)highestPriorityNotCheckedCourseId!).Result.Count()
                < _courseRepository.GetCourseByIdAsync((int)highestPriorityNotCheckedCourseId!).Result.Quota)
            {
                await _studentOnCourseRepository.AddStudentToCourseAsync((int)currentStudent.Id!, (int)highestPriorityNotCheckedCourseId!);              
                unassignedStudents.Remove(currentStudent);
                continue;
            }

            
            var studentsOnCourseIds = _studentOnCourseRepository.GetStudentsIdsByCourseIdAsync((int)highestPriorityNotCheckedCourseId!).Result;

            var studentWithLowestAverageGrade = _studentRepository.GetAllStudentsAsync().Result
                .Where(s => studentsOnCourseIds.Contains((int)s.Id!))
                .MinBy(s => s.AverageGrade)!;

            if (studentWithLowestAverageGrade.AverageGrade > currentStudent.AverageGrade)
            {
                await _studentOnCourseRepository.MarkCourseAsChecked((int)currentStudent.Id, (int)highestPriorityNotCheckedCourseId);
            }

            await _studentOnCourseRepository.MarkCourseAsChecked((int)studentWithLowestAverageGrade.Id, (int)highestPriorityNotCheckedCourseId);

            // todo
            
            unassignedStudents.Remove(currentStudent);
            unassignedStudents.Add(studentWithLowestAverageGrade);

            await _studentOnCourseRepository.RemoveStudentFromCourseAsync((int)studentWithLowestAverageGrade.Id, (int)highestPriorityNotCheckedCourseId);
            await _studentOnCourseRepository.AddStudentToCourseAsync((int)currentStudent.Id, (int)highestPriorityNotCheckedCourseId);
        }
    }

    public async Task<IEnumerable<Course>> GetStudentCourses(int studentId)
    {
        return await _studentOnCourseRepository.GetCoursesByStudentIdAsync(studentId);
    }
}
