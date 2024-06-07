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
  studentCourses: Course[] = [];
  studentPriorities: StudentPriorities = {
    studentName: '',
    coursesPriorities: {}
  };

  allCourses: Course[] = [];

  availableCoursesByStudyComponent: AvailableCoursesResponse = {};

  coursePrioritiesToSet: { [studyComponent: string]: { [courseTitle: string]: number } } = {};

  constructor(private studentsService: StudentsService,
    private coursesService: CoursesService, private route: ActivatedRoute)
  {

  }

  ngOnInit(): void {  
    const id = this.route.snapshot.paramMap.get('id') ?? '';
    this.loadStudent(id);

    // this.loadAvailableCoursesGroups();
    // this.initializeCoursePriorities();

    this.loadStudentPriorities();

    //this.loadCourses();
    //this.loadStudentCourses();
    
  }

  loadStudent(id: string): void {
    this.studentsService.getStudent(id).subscribe(data => {
      this.student = data;

      this.loadAvailableCoursesGroups();
    });
  }

  loadStudentCourses(): void {
    const id = this.route.snapshot.paramMap.get('id') ?? '';
    this.studentsService.getStudentCourses(id).subscribe(data => {
      console.log('Student courses:');
      console.log(data);
      this.studentCourses = data; // Wrap data in an array
    });
  }

  loadStudentPriorities(): void {
    const id = this.route.snapshot.paramMap.get('id') ?? '';

    // TODO: check if it's called only once as expected
    this.coursesService.isPlacementExecuted().subscribe(isExecuted => {
      if (isExecuted) {
      this.studentsService.getStudentPriorities(id).subscribe(data => {
        console.log('Student priorities:');
        console.log(data);
        this.studentPriorities = data;
      });
      }
    });
  }
  
  getCourseId(courseTitle: string): string {
    let courseId = '';
    this.coursesService.getCourseId(courseTitle).subscribe(id => {
      courseId = id;
    });
    return courseId;
  }

    // async getCourseId(courseTitle: string): Promise<string> {
    //   let courseId = '';
    //   for (const course of await this.coursesService.getCourses()) {
    //     if (course.title === courseTitle) {
    //       courseId = course.id;
    //       return courseId;
    //     }
    //   }
    //   return courseId;
    // }

    // this.coursesService.getCourses().subscribe(courses => {

    //   console.log("searching courses:")
    //   for (const course of courses) {

    //     console.log(course);
    //     if (course.title === courseTitle) {
    //       courseId = course.id;
    //       return courseId;
    //     }
    //   }
    //   return courseId;
    // });

  //   return courseId;
  // }
  
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

  // setCoursePriorities(): void {
  //   const availableCoursesGroups = this.studentsService.getAvailableCoursesGroups(this.student.id);
  //   availableCoursesGroups.subscribe(groups => {
  //     groups.forEach((value, studyComponent) => {
  //       this.coursePrioritiesToSet[studyComponent] = {};
  //       value.forEach(course => {
  //         this.coursePrioritiesToSet[studyComponent][course.title] = 0; // Default priority
  //       });
  //     });
  //   });
  // }

  isStudentEnrolled(courseTitle: string): boolean {
    return this.studentCourses.some(course => course.title === courseTitle);
  }

  isPlacementExecuted(): boolean {
    console.log('Check isPlacementExecuted:');
    
    this.coursesService.isPlacementExecuted().subscribe(isExecuted => {
      console.log('Is placement executed:', isExecuted);
      return isExecuted;
    });

    return false;
  }

  submitPriorities(): void {
    const studentPriorities = {
      studentName: this.student.name,
      coursesPriorities: this.coursePrioritiesToSet
    };

    this.studentsService.submitStudentPriorities(this.student.id, studentPriorities).subscribe(response => {
      console.log('Priorities submitted successfully', response);
    });
  }

  loadAvailableCoursesGroups(): void {
    this.studentsService.getAvailableCoursesGroups(this.student.id).subscribe(CoursesByStudyComponent => {
      console.log('Available courses by study coponent:');
      console.log(CoursesByStudyComponent);
      this.availableCoursesByStudyComponent = CoursesByStudyComponent;
      this.initializeCoursePriorities();
    });
  }

  loadCourses(): void {
    this.coursesService.getCourses().subscribe(courses => {
      this.allCourses = courses;
    });
  }

  initializeCoursePriorities(): void {
    for (const studyComponent in this.availableCoursesByStudyComponent) {
      this.coursePrioritiesToSet[studyComponent] = {};
      console.log('Study component:', studyComponent);
      for (const course of this.availableCoursesByStudyComponent[studyComponent]!) {
        console.log('Course:', course);
        this.coursePrioritiesToSet[studyComponent][course.title] = 0; // Default priority
      }
    }
  }

  generatePriorityOptions(numOptions: number): number[] {
    return Array.from({ length: numOptions }, (_, index) => index + 1);
  }
}

