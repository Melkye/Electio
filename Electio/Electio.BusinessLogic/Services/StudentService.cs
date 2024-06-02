using AutoMapper;
using Electio.BusinessLogic.DTOs;
using Electio.DataAccess;
using Electio.DataAccess.Entities;
using Electio.DataAccess.Enums;
using Electio.DataAccess.Repositories;

namespace Electio.BusinessLogic.Services;
public class StudentService
{
    private readonly IMapper _mapper;
    //private readonly IUnitOfWork _unitOfWork;
    private readonly UnitOfWork _unitOfWork;

    public StudentService(
        IMapper mapper,
        UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentGetDTO> Create(StudentCreateDTO dto)
    {
        Student student = _mapper.Map<Student>(dto);
        student.Id = Guid.NewGuid();

        student = await _unitOfWork.StudentRepository.CreateStudentAsync(student);

        return _mapper.Map<StudentGetDTO>(student);
    }

    // TODO: resolve async/await issue
    // TODO: add a batch creation method instead of one by one
    public async Task CreateRandomStudentsAsync()
    {
        //var createdStudents = new List<StudentGetDTO>();
        if (!(await _unitOfWork.StudentRepository.GetAllAsync()).Any())
        {
            var studentsFirstStudyYear = _mapper.Map<IEnumerable<Student>>(
                Generator.GenerateStudentsForStudyYear(studyYear: StudyYear.First));

            var studentsSecondStudyYear = _mapper.Map<IEnumerable<Student>>(
                Generator.GenerateStudentsForStudyYear(studyYear: StudyYear.Second));

            await _unitOfWork.StudentRepository.CreateStudentsAsync(
                studentsFirstStudyYear.Union(studentsSecondStudyYear));
        }

        await _unitOfWork.SaveChangesAsync();
    }

    // TODO: change from StudentOnCourse
    public async Task<List<StudentOnCourse>> SetPriorities(StudentPrioritiesDTO dto)
    {
        var student = (await _unitOfWork.StudentRepository.GetAllAsync()).First(s => s.Name == dto.StudentName);

        var courses = await _unitOfWork.CourseRepository.GetAllAsync();

        var coursePriorities = dto.CoursesPriorities.SelectMany(
            componentCoursesPriorities => componentCoursesPriorities.Value.Select(
                coursePriority =>
                    new {
                        CourseTitle = coursePriority.Key,
                        Priority = coursePriority.Value
                    }
                ));

        var studentsOnCoursesAfterPrioritiesSet = new List<StudentOnCourse>();
        foreach (var coursePriority in coursePriorities)
        {
            var courseId = courses.First(c => c.Title == coursePriority.CourseTitle).Id;
            studentsOnCoursesAfterPrioritiesSet.Add(
                await _unitOfWork.StudentOnCourseRepository.AddCoursePriorityToStudent(student.Id, courseId, coursePriority.Priority));
        }

        await _unitOfWork.SaveChangesAsync();

        return studentsOnCoursesAfterPrioritiesSet;
    }

    public async Task<IEnumerable<IEnumerable<StudentOnCourse>>> SetRandomPriorities()
    {
        // TODO: get rid of mapping here | create a special servise foe placement?
        var students = await GetAllAsync();
        var studentsDTOs = _mapper.Map<List<StudentGetDTO>>(students);
        var courses = await _unitOfWork.CourseRepository.GetAllAsync();
        var coursesDTOs = _mapper.Map<List<CourseGetDTO>>(courses);

        var dtos = Generator.GenerateStudentPriorities(studentsDTOs, coursesDTOs);

        var results = new List<List<StudentOnCourse>>();
        foreach(var dto in dtos)
        {
            results.Add(await SetPriorities(dto));
        }

        return results;
    }

    public async Task<IEnumerable<StudentGetDTO>> GetAllAsync()
    {
        var students = await _unitOfWork.StudentRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<StudentGetDTO>>(students);
    }

    public async Task<StudentGetDTO> GetByIdAsync(Guid id)
    {
        var student = await _unitOfWork.StudentRepository.GetByIdAsync(id);

        return _mapper.Map<StudentGetDTO>(student);
    }

    // TODO: consider moving it to StudentOnCourseService
    public async Task ExecuteGradeBasedPlacement(StudyComponent studyComponent)
    {
        // NOTE: courses in Student should be ordered from highest(1) to lowest(len) priority
        // NOTE: consider ordering from lowest to highest to remove at last pos
        //public List<Course> GradeBasedPlacement(List<Student> students)

        var students = await _unitOfWork.StudentRepository.GetAllAsync();
        // TODO: get rid of this variable
        var unassignedStudents = new List<Student>(students);

        while (unassignedStudents.Any())
        {
            var currentStudent = unassignedStudents.Last();

            // TODO: fix algo | this is now priotities by SK
            var studentCoursesPriotities = await _unitOfWork.StudentOnCourseRepository
                .GetCoursesWithPrioritiesByStudentIdAsync(currentStudent.Id, studyComponent);

            // TODO: hack to filter students who don't choose the specified study component
            // TODO: should update the unassigned students list to get only those students
            // who's study year matches the study conponent
            if (!studentCoursesPriotities.Any())
            {
                unassignedStudents.Remove(currentStudent);
                continue;
            }

            var highestPriorityNotCheckedCourseId = studentCoursesPriotities
                .Where(soc => soc.IsChecked == false)
                .OrderBy(soc => soc.Priority).First().CourseId;

            //var course = placement.First(course => course.Title == highestPriorityCourseId);
            var countOfStudentsEnrolledOnCourse = (await _unitOfWork.StudentOnCourseRepository.GetStudentsByCourseIdAsync(highestPriorityNotCheckedCourseId)).Count();
            var courseQuota = (await _unitOfWork.CourseRepository.GetCourseByIdAsync(highestPriorityNotCheckedCourseId)).Quota;

            if (countOfStudentsEnrolledOnCourse < courseQuota)
            {
                var tempStudOnCourse = await _unitOfWork.StudentOnCourseRepository.AddStudentToCourseAsync(currentStudent.Id, highestPriorityNotCheckedCourseId);
                // TODO: remove savve here and mmake it in the end of the method
                
                
                //-------------------------------- remove save
                await _unitOfWork.SaveChangesAsync(); 
                unassignedStudents.Remove(currentStudent);
                continue;
            }


            var studentsOnCourseIds = (await _unitOfWork.StudentOnCourseRepository.GetStudentsByCourseIdAsync(highestPriorityNotCheckedCourseId))
                .Select(s => s.Id).ToList();

            var studentWithLowestAverageGrade = _unitOfWork.StudentRepository.GetAllAsync().Result
                .Where(s => studentsOnCourseIds.Contains(s.Id!))
                .MinBy(s => s.AverageGrade)!;

            if (studentWithLowestAverageGrade.AverageGrade > currentStudent.AverageGrade)
            {
                await _unitOfWork.StudentOnCourseRepository.MarkCourseAsChecked(currentStudent.Id, highestPriorityNotCheckedCourseId);
                // TODO: remove savve here and mmake it in the end of the method
                // await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.StudentOnCourseRepository.MarkCourseAsChecked(studentWithLowestAverageGrade.Id, highestPriorityNotCheckedCourseId);
            // TODO: remove savve here and mmake it in the end of the method
            //await _unitOfWork.SaveChangesAsync();

            // todo

            unassignedStudents.Remove(currentStudent);
            unassignedStudents.Add(studentWithLowestAverageGrade);

            await _unitOfWork.StudentOnCourseRepository.RemoveStudentFromCourseAsync(studentWithLowestAverageGrade.Id, highestPriorityNotCheckedCourseId);
            // TODO: remove savve here and mmake it in the end of the method
            //await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.StudentOnCourseRepository.AddStudentToCourseAsync(currentStudent.Id, highestPriorityNotCheckedCourseId);
            // TODO: remove savve here and mmake it in the end of the method
            //await _unitOfWork.SaveChangesAsync();
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseGetDTO>> GetStudentCoursesAsync(Guid studentId)
    {
        var courses = await _unitOfWork.StudentOnCourseRepository.GetEnrolledCoursesByStudentIdAsync(studentId);
        return _mapper.Map<IEnumerable<CourseGetDTO>>(courses);
    }

    public async Task<StudentPrioritiesDTO> GetStudentPrioritiesAsync(Guid studentId)
    {
        var studyComponents = Enum.GetValues<StudyComponent>();
        var studentOnCourses = new List<StudentOnCourse>();

        var studentPriorities = new StudentPrioritiesDTO
        {
            StudentName = (await _unitOfWork.StudentRepository.GetByIdAsync(studentId)).Name,
            CoursesPriorities = new Dictionary<StudyComponent, Dictionary<string, int>>()
        };

        foreach (var studyComponent in studyComponents)
        {
            var coursesPriotities = 
                _unitOfWork.StudentOnCourseRepository.GetCoursesWithPrioritiesByStudentIdAsync(studentId, studyComponent)
                .Result
                .ToDictionary(soc => 
                    _unitOfWork.CourseRepository.GetAllAsync().Result.First(c => c.Id == soc.CourseId).Title,
                    soc => soc.Priority);

            if (coursesPriotities.Any())
                studentPriorities.CoursesPriorities.Add(studyComponent, coursesPriotities);
        }

        return studentPriorities;

        //var studentPriorities = new StudentPrioritiesDTO
        //{
        //    StudentName = (await _unitOfWork.StudentRepository.GetByIdAsync(studentId)).Name,
        //    CoursesPriorities = studentOnCourses
        //        .GroupBy(soc => soc.Course.StudyComponent)
        //        .ToDictionary(
        //            group => group.Key,
        //            group => group.ToDictionary(
        //                soc => soc.Course.Title,
        //                soc => soc.Priority))
        //};
    }

    public async Task<IDictionary<StudyComponent, List<string>>> GetAvailableCourses(Guid id)
    {
        var studyYear = _unitOfWork.StudentRepository.GetByIdAsync(id).Result.StudyYear;

        var availableStudyComponents = Generator.GetStudyComponentsAvailableToStudyYear(studyYear);

        var courses = await _unitOfWork.CourseRepository.GetAllAsync();

        var availableCourses = courses
            .GroupBy(c => c.StudyComponent, c => c.Title)
            .Where(group => availableStudyComponents.Contains(group.Key))
            .ToDictionary(group => group.Key, group => group.ToList());

        return availableCourses;
    }
}
