using Bogus;
using Bogus.Distributions.Gaussian;
using Electio.BusinessLogic.DTOs;
using Electio.DataAccess.Entities;
using Electio.DataAccess.Enums;

// TODO: move Specialty and other enums to a separate Common project | maybe Generatior too
namespace Electio.BusinessLogic;

public static class Generator
{
    // TODO: consider counting how many courses to generate by counting who will enroll
    public static List<CourseCreateDTO> GenerateCoursesForStudyComponent(
        int numberOfCourses = 5,
        Faculty faculty = Faculty.FICE,
        List<Specialty> specialties = default,
        StudyComponent studyComponent = default,
        int minQuota = 30,
        int maxQuota = 120)
    {
        if (specialties == default)
        {
            specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 };
        }
        if (studyComponent == default)
        {
            studyComponent = StudyComponent.SK1;
        }

        var coursesFaker = new Faker<CourseCreateDTO>("uk")
            .Rules((f, c) =>
            {
                c.Title = "Дисципліна " + f.Music.Random.Word();
                c.Quota = 18; // f.Random.Int(minQuota, maxQuota);
                c.Specialties = specialties;
                c.Faculty = faculty;
                c.StudyComponent = studyComponent;
            });

        return coursesFaker.Generate(numberOfCourses);

        return
        [
            new CourseCreateDTO
            {
                Title = "dotnet",
                Quota = 34,
                Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
                Faculty = Faculty.FICE,
                StudyComponent = StudyComponent.SK1
            },
            new CourseCreateDTO
            {
                Title = "node,js",
                Quota = 33,
                Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
                Faculty = Faculty.FICE,
                StudyComponent = StudyComponent.SK1
            },
            new CourseCreateDTO
            {
                Title = "java",
                Quota = 33,
                Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
                Faculty = Faculty.FICE,
                StudyComponent = StudyComponent.SK1
            }
        ];
    }

    public static List<StudentCreateDTO> GenerateStudentsForStudyYear(
        int numberOfStudents = 90,
        List<Specialty> specialties = default,
        Faculty faculty = Faculty.FICE,
        StudyYear studyYear = StudyYear.First,
        int averageGradeMean = 80,
        int averageGradeStdDev = 10)
    {
        if (specialties == default)
        {
            specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 };
        }

        var studentsFaker = new Faker<StudentCreateDTO>("uk")
            .Rules((f, s) =>
            {
                s.Name = f.Random.Bool()
                    ? f.Name.FullName(gender: Bogus.DataSets.Name.Gender.Male)
                    : f.Name.FullName(gender: Bogus.DataSets.Name.Gender.Female);
                s.AverageGrade = Math.Max(60,
                    Math.Min(f.Random.GaussianDouble(averageGradeMean, averageGradeStdDev), 100));
                // TODO: make specitlities look realistic
                s.Specialty = f.PickRandom(specialties);
                s.Faculty = faculty;
                s.StudyYear = studyYear;
            });

        return studentsFaker.Generate(numberOfStudents);
    }

    public static List<StudentPrioritiesDTO> GenerateStudentPriorities(List<StudentGetDTO> students, List<CourseGetDTO> courses)
    {
        var studyComponentCapacities = courses
            .GroupBy(c => c.StudyComponent)
            .ToList()
            .Select(group => group.Sum(c => c.Quota));

        if (studyComponentCapacities.Any(capacity => capacity < students.Count/studyComponentCapacities.Count()))
        {
            // TODO: resolve issue with the number of students and quotas
            throw new ArgumentException("Number of students must be equal to the sum of quotas of all courses in a study component");
        }

        var allStudentsPriorities = new List<StudentPrioritiesDTO>();

        foreach (var student in students)
        {
            allStudentsPriorities.Add(GenerateStudentPriorities(student, courses));
        }

        return allStudentsPriorities;
    }

    public static StudentPrioritiesDTO GenerateStudentPriorities(StudentGetDTO student, List<CourseGetDTO> courses)
    {
        var studentPriorities = new StudentPrioritiesDTO
        {
            StudentName = student.Name,
        };

        var studyComponentsAvailableToStudent = Helper.GetStudyComponentsAvailableToStudyYear(student.StudyYear);

        var coursesAvailableToStudent = courses
            .Where(c => 
                c.Faculty == student.Faculty
                && c.Specialties.Contains(student.Specialty)
                && studyComponentsAvailableToStudent.Contains(c.StudyComponent))
            .ToList();

        var coursesByStudyComponent = coursesAvailableToStudent
            .GroupBy(c => c.StudyComponent)
            .ToDictionary(group => group.Key, group => group.ToList());


        foreach (var (studyComponent, componentCourses) in coursesByStudyComponent)
        {
            var availablePriorities = new Faker().Random.Shuffle(Enumerable.Range(1, componentCourses.Count)).ToList();

            studentPriorities.CoursesPriorities.Add(
                studyComponent,
                componentCourses.ToDictionary(
                    c => c.Title, 
                    c =>
                    {  
                        var priority = availablePriorities.Last();
                        availablePriorities.RemoveAt(availablePriorities.Count - 1);
                        return priority;
                    })
                );
        }

        return studentPriorities;
    }
}