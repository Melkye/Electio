//using Bogus;
//using Bogus.Distributions.Gaussian;
//using Electio.BusinessLogic.DTOs;
//using Electio.DataAccess.Entities;
//using Electio.DataAccess.Enums;

//// TODO: move Specialty and other enums to a separate Common project | maybe Generatior too
//namespace Electio.Generation;

//public static class Generator
//{
//    // TODO: consider counting how many courses to generate by counting who will enroll
//    public static List<CourseCreateDTO> GenerateCoursesForFaculty(
//        int numberOfCourses,
//        Faculty faculty,
//        List<Specialty> specialties,
//        List<StudyComponent> studyComponents,
//        int minQuota = 30,
//        int maxQuota = 120)
//    {
//        //var coursesFaker = new Faker<CourseCreateDTO>("uk_UA")
//        //    .Rules((f, c) =>
//        //    {
//        //        c.Title = "Дисципліна" + f.Lorem.Word();
//        //        c.Quota = f.Random.Int(minQuota, maxQuota);
//        //        c.Specialties = specialties;
//        //        c.Faculty = faculty;
//        //        c.StudyComponent = f.PickRandom(studyComponents);
//        //    });

//        //return coursesFaker.Generate(numberOfCourses);

//        return
//        [
//            new CourseCreateDTO
//            {
//                Title = "dotnet",
//                Quota = 3,
//                Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
//                Faculty = Faculty.FICE,
//                StudyComponent = StudyComponent.SK1
//            },
//            new CourseCreateDTO
//            {
//                Title = "node,js",
//                Quota = 3,
//                Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
//                Faculty = Faculty.FICE,
//                StudyComponent = StudyComponent.SK1
//            },
//            new CourseCreateDTO
//            {
//                Title = "java",
//                Quota = 3,
//                Specialties = new List<Specialty> { Specialty.IT121, Specialty.IT123, Specialty.IT126 },
//                Faculty = Faculty.FICE,
//                StudyComponent = StudyComponent.SK1
//            }
//        ];
//    }

//    public static List<StudentCreateDTO> GenerateStudents(
//        int numberOfStudents,
//        List<Specialty> specialties,
//        Faculty faculty,
//        StudyYear studyYear,
//        int averageGradeMean = 80,
//        int averageGradeStdDev = 10)
//    {
//        var studentsFaker = new Faker<StudentCreateDTO>("uk_UA")
//            .Rules((f, s) =>
//            {
//                s.Name = f.Random.Bool()
//                    ? "Студент" + f.Name.FullName(gender: Bogus.DataSets.Name.Gender.Male)
//                    : "Студентка" + f.Name.FullName(gender: Bogus.DataSets.Name.Gender.Female);
//                s.AverageGrade = f.Random.GaussianDouble(averageGradeMean, averageGradeStdDev);
//                // TODO: make specitlities look realistic
//                s.Specialty = f.PickRandom(specialties);
//                s.Faculty = faculty;
//                s.StudyYear = studyYear;
//            });

//        return studentsFaker.Generate(numberOfStudents);
//    }

//    public static List<StudentPrioritiesDTO> GenerateStudentPriorities(List<StudentGetDTO> students, List<CourseGetDTO> courses)
//    {
//        var studyComponentCapacities = courses
//            .GroupBy(c => c.StudyComponent)
//            .ToList()
//            .Select(group => group.Sum(c => c.Quota));

//        if (studyComponentCapacities.Any(capacity => capacity < students.Count))
//        {
//            // TODO: resolve issue with the number of students and quotas
//            throw new ArgumentException("Number of students must be equal to the sum of quotas of all courses in a study component");
//        }

//        var allStudentsPriorities = new List<StudentPrioritiesDTO>();

//        foreach (var student in students)
//        {
//            allStudentsPriorities.Add(GenerateStudentPriorities(student, courses));
//        }

//        return allStudentsPriorities;
//    }

//    public static StudentPrioritiesDTO GenerateStudentPriorities(StudentGetDTO student, List<CourseGetDTO> courses)
//    {
//        var studentPriorities = new StudentPrioritiesDTO
//        {
//            StudentName = student.Name,
//        };

//        var coursesByStudyComponent = courses
//            .GroupBy(c => c.StudyComponent)
//            .ToDictionary(group => group.Key, group => group.ToList());

//        foreach (var (studyComponent, componentCourses) in coursesByStudyComponent)
//        {
//            var availablePriorities = new Faker().Random.Shuffle(Enumerable.Range(1, componentCourses.Count)).ToList();

//            studentPriorities.CoursesPriorities.Add(
//                studyComponent,
//                componentCourses.ToDictionary(
//                    c => c.Title, 
//                    c =>
//                    {  
//                        var priority = availablePriorities.Last();
//                        availablePriorities.RemoveAt(availablePriorities.Count - 1);
//                        return priority;
//                    })
//                );
//        }

//        return studentPriorities;
//    }
//}