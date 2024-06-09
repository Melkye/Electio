import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StudentsService } from '../../services/students.service';
import { Student } from '../../models/student.model';
import { Course } from '../../models/course.model';
import { AvailableCoursesResponse } from '../../models/course.model';
import { StudentPriorities } from '../../models/student-priorities.model';
import { CoursesService } from '../../services/courses.service';

@Component({
  selector: 'app-student-detail',
  templateUrl: './student-detail.component.html',
})
export class StudentDetailComponent implements OnInit {
  student: Student = { 
    id: 'abubaId', 
    name: 'abubaName', 
    averageGrade: 10000,
    faculty: 1000, //'abuba',
    specialty: 1000, //'abuba',
    studyYear: 10000
  };

  isPlacementExecuted = false;

  allCourses: Course[] = [];

  availableCoursesByStudyComponent: AvailableCoursesResponse = {};

  coursePrioritiesToSet: { [studyComponent: string]: { [courseTitle: string]: number } } = {};
  
  studentCourses: Course[] = [];

  studentPriorities: StudentPriorities = {
    studentName: '',
    coursesPriorities: {}
  };

  constructor(private studentsService: StudentsService,
    private coursesService: CoursesService, private route: ActivatedRoute)  {  }

  ngOnInit(): void {  
    const id = this.route.snapshot.paramMap.get('id') ?? '';

    // this.loadAvailableCoursesGroups();
    // this.initializeCoursePriorities();

    this.loadStudent(id);
    
    this.coursesService.setIsPlacementExecuted().subscribe(isExecuted => {
      this.isPlacementExecuted = isExecuted;


      ////////////
      if (isExecuted) {
        console.log('Placement has been executed. Loading student courses.');
        this.loadStudentCourses();
      }
      else {
        console.log("Placement hasn't been executed. Setting student priorities and courses.");
        this.loadStudentPriorities()
      }
    });
    
    //this.loadStudentPriorities();
    //this.loadCourses();
    //this.loadStudentCourses();
    
  }

  loadStudent(id: string): void {
    this.studentsService.getStudent(id).subscribe(data => {
      this.student = data;

      this.loadAvailableCoursesPerStudyComponent();
      this.loadStudentPriorities();


    });
  }

  loadStudentCourses(): void {
    const id = this.route.snapshot.paramMap.get('id') ?? '';
    this.studentsService.getStudentCourses(id).subscribe(data => {
      console.log('Student courses:');
      console.log(data);
      this.studentCourses = data;
    });
  }

  loadStudentPriorities(): void {
    // TODO: when course is created, initialise priorirites to 0; (on back)
    const id = this.route.snapshot.paramMap.get('id') ?? '';
      this.studentsService.getStudentPriorities(id).subscribe(data => {
          console.log('Student priorities:');
          console.log(data);

          if (Object.keys(data.coursesPriorities).length === 0) {
            
            console.log('Student priorities not set. Setting them to zeros.');
            this.initializeCoursePriorities();
          }
          else {
            console.log('Student priorities already set, loading them.');
            this.studentPriorities = data;
          }
      });
  }
  
  getCourseId(courseTitle: string): string {
    return this.allCourses.find(course => course.title === courseTitle)?.id!;
  }
  
  getCoursePriority(courseId: string): number {
    let priority = 0;
    for (const studyComponent in this.studentPriorities?.coursesPriorities) {
      if (this.studentPriorities.coursesPriorities[studyComponent][courseId] !== undefined) {
        priority = this.studentPriorities.coursesPriorities[studyComponent][courseId];
        return priority;
      }
    }
    return priority;
  }

  isStudentEnrolled(courseTitle: string): boolean {
    const isEnrolled = this.studentCourses.some(course => course.title === courseTitle);
    console.log('Checking if student is enrolled in course:', courseTitle, isEnrolled);
    return isEnrolled;
  }

  submitPriorities(): void {
    // const studentPriorities = {
    //   studentName: this.student.name,
    //   coursesPriorities: this.coursePrioritiesToSet
    // };

    this.studentsService.submitStudentPriorities(this.student.id, this.studentPriorities).subscribe(response => {
      console.log('Priorities submitted successfully', response);
    });
  }

  loadAvailableCoursesPerStudyComponent(): void {
    this.studentsService.getAvailableCoursesPerStudyComponent(this.student.id).subscribe(coursesByStudyComponent => {
      console.log('Available courses by study component:');
      console.log(coursesByStudyComponent);
      this.availableCoursesByStudyComponent = coursesByStudyComponent;
    });
  }

  loadCourses(): void {
    this.coursesService.getCourses().subscribe(courses => {
      this.allCourses = courses;
    });
  }

  initializeCoursePriorities(): void {
    this.studentPriorities.studentName = this.student.name;
    for (const studyComponent in this.availableCoursesByStudyComponent) {
      this.studentPriorities.coursesPriorities[studyComponent] = {};
      console.log('Setting study component: ', studyComponent);
      const courses = this.availableCoursesByStudyComponent[studyComponent];
      for (const course of courses) {
        console.log('Study component:', studyComponent, 'Course:', course, "Priority: 2147483647 (max c# int)");
        this.studentPriorities.coursesPriorities[studyComponent][course.title] = 2147483647; // Default priority, max c# int
      }
    }
  }

  generatePriorityOptions(numOptions: number): number[] {
    return Array.from({ length: numOptions }, (_, index) => index + 1);
  }
}

