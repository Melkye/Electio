using System.Collections.Generic;
using System.Linq;
using Bogus;

using Electio.BusinessLogic.DTOs;
using Electio.DataAccess.Entities;
using Electio.DataAccess.Enums;

// TODO: move Specialty and other enums to a separate Common project | maybe Generatior too
namespace Electio.BusinessLogic;

public static class Generator
{
    public static List<CourseCreateDTO> GenerateCoursesForStudyComponent(
        int numberOfCourses = 5,
        int numberOfStudents = default, // count of students is passed
        Faculty faculty = Faculty.FICE,
        List<Specialty> specialties = default,
        StudyComponent studyComponent = default
        //int minQuota = 30,
        //int maxQuota = 120
        )
    {
        if (specialties == default)
        {
            specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 };
        }
        if (studyComponent == default)
        {
            studyComponent = StudyComponent.SK1;
        }

        var generatedCourses = new List<CourseCreateDTO>();

        var quotaEquallyDistributed = (int)Math.Ceiling((double)numberOfStudents / numberOfCourses);

        var percentageOfHighDemandCourses = 0.3;
        var percentageOfHighDemandCoursesQuotaComparedToEquallyDistributed = 0.7;

        var numberOfHighDemandCourses = (int)Math.Ceiling(numberOfCourses * percentageOfHighDemandCourses);
        var quotaForHighDemandCourses = (int)Math.Ceiling(quotaEquallyDistributed * percentageOfHighDemandCoursesQuotaComparedToEquallyDistributed);
        var highDemandCoursesFaker = new Faker<CourseCreateDTO>("uk")
            .Rules((f, c) =>
            {
                c.Title = "Дисципліна " + f.Music.Random.Word() + " " + f.Music.Random.Word();
                c.Quota = quotaForHighDemandCourses;
                c.Specialties = specialties;
                c.Faculty = faculty;
                c.StudyComponent = studyComponent;
            });
        var highDemandCourses = highDemandCoursesFaker.Generate(numberOfHighDemandCourses);


        var numberOfNonHighDemandCourses = numberOfCourses - numberOfHighDemandCourses;
        var leftoversFromHighDemandCourses = numberOfStudents - (quotaForHighDemandCourses * numberOfHighDemandCourses);
        var quotaForNonHighDemandCourses = (int)Math.Ceiling((double)leftoversFromHighDemandCourses / numberOfNonHighDemandCourses);
        var nonHighDemandCoursesFaker = new Faker<CourseCreateDTO>("uk")
            .Rules((f, c) =>
            {
                c.Title = "Дисципліна " + f.Music.Random.Word() + " " + f.Music.Random.Word();
                c.Quota = quotaForNonHighDemandCourses;
                c.Specialties = specialties;
                c.Faculty = faculty;
                c.StudyComponent = studyComponent;
            });
        var nonHighDemandCourses = nonHighDemandCoursesFaker.Generate(numberOfNonHighDemandCourses);

        //generatedCourses.AddRange(highDemandCourses);
        generatedCourses.AddRange(nonHighDemandCourses);
        generatedCourses.AddRange(highDemandCourses);

        return generatedCourses;
        //return
        //[
        //    new CourseCreateDTO
        //    {
        //        Title = "dotnet",
        //        Quota = 34,
        //        Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
        //        Faculty = Faculty.FICE,
        //        StudyComponent = StudyComponent.SK1
        //    },
        //    new CourseCreateDTO
        //    {
        //        Title = "node,js",
        //        Quota = 33,
        //        Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
        //        Faculty = Faculty.FICE,
        //        StudyComponent = StudyComponent.SK1
        //    },
        //    new CourseCreateDTO
        //    {
        //        Title = "java",
        //        Quota = 33,
        //        Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
        //        Faculty = Faculty.FICE,
        //        StudyComponent = StudyComponent.SK1
        //    }
        //];
    }

    public static List<StudentCreateDTO> GenerateStudentsForStudyYear(
        int numberOfStudents = 100,
        List<Specialty> specialties = default,
        Faculty faculty = Faculty.FICE,
        StudyYear studyYear = StudyYear.First,
        int averageGradeMean = 80,
        int averageGradeStdDev = 8)
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

    public static List<StudentPrioritiesDTO> GenerateStudentPriorities(List<StudentGetDTO> students, List<CourseGetDTO> courses, bool isCloseToReal)
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
            if (isCloseToReal)
                allStudentsPriorities.Add(GenerateStudentPrioritiesCloseToReal(student, courses));
            else
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
                studyComponentsAvailableToStudent.Contains(c.StudyComponent)
                && c.Faculty == student.Faculty
                && c.Specialties.Contains(student.Specialty));

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
                        availablePriorities.Remove(priority);
                        return priority;
                    })
                );
        }

        return studentPriorities;
    }

    public static StudentPrioritiesDTO GenerateStudentPrioritiesCloseToReal(StudentGetDTO student, List<CourseGetDTO> courses)
    {
        var studentPriorities = new StudentPrioritiesDTO
        {
            StudentName = student.Name,
        };

        var studyComponentsAvailableToStudent = Helper.GetStudyComponentsAvailableToStudyYear(student.StudyYear);

        var coursesAvailableToStudent = courses
            .Where(c =>
                studyComponentsAvailableToStudent.Contains(c.StudyComponent)
                && c.Faculty == student.Faculty
                && c.Specialties.Contains(student.Specialty));

        var coursesByStudyComponent = coursesAvailableToStudent
            .GroupBy(c => c.StudyComponent)
            .ToDictionary(group => group.Key, group => group.ToList());

        var highDemandCourses = coursesByStudyComponent
            .Select(kvp => new { StudyComponent = kvp.Key, Courses = kvp.Value.Take(kvp.Value.Count / 3)})
            .ToDictionary(kvp => kvp.StudyComponent, kvp => kvp.Courses);

        foreach (var (studyComponent, componentCourses) in coursesByStudyComponent)
        {
            var highDemandPriorities = new Faker().Random.Shuffle(Enumerable.Range(1, highDemandCourses[studyComponent].Count())).ToList();

            var notHighDemandPriorities = 
                new Faker().Random.Shuffle(Enumerable.Range(1, componentCourses.Count))
                .Except(highDemandPriorities).ToList();

            studentPriorities.CoursesPriorities.Add(
                studyComponent,
                componentCourses.ToDictionary(
                    c => c.Title,
                    c =>
                    {
                        var priority = int.MaxValue;
                        if (highDemandCourses[studyComponent].Contains(c))
                        {
                            priority = highDemandPriorities.Last();
                            highDemandPriorities.Remove(priority);
                        }
                        else
                        {
                            priority = notHighDemandPriorities.Last();
                            notHighDemandPriorities.Remove(priority);
                        }
                        return priority;
                    })
                );
        }

        return studentPriorities;
    }
}