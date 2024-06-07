// course-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Course } from '../../models/course.model';
import { CourseEnrollment } from '../../models/course-enrollment.model';
import { CoursesService } from '../../services/courses.service';
import { concatMap } from 'rxjs';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  courses: Course[] = [];
  courseEnrollments: CourseEnrollment[] = [];
  isPlacementExecuted = false;

  constructor(private coursesService: CoursesService) {}
  ngOnInit(): void {
    this.loadCourses();
    this.loadCourseEnrollments();
    
    this.coursesService.setIsPlacementExecuted().subscribe(isExecuted => {
      this.isPlacementExecuted = isExecuted;
    });

  }

  // TODO: check if it works as Promise and not as Observable
  loadCourses(): void {
    this.coursesService.getCourses().subscribe(courses => {
      this.courses = courses;
    });
  }

  loadCourseEnrollments(): void {
    this.coursesService.getAllCoursesPlacement().subscribe(enrollments => {
      this.courseEnrollments = enrollments;
    });
  }

  getEnrollmentsForCourse(course: Course): CourseEnrollment | undefined {
    return this.courseEnrollments.find(enrollment => enrollment.title === course.title);
  }

  runPlacementAlgorithm(): void {
    this.coursesService.unenrollEveryone().pipe(
      concatMap(() => {
        console.log('Everyone unenrolled');
        return this.coursesService.executePlacement();
      })
    ).subscribe(() => {
      console.log('Placement algorithm executed');
      //this.isPlacementExecuted = true;  // Set a flag to indicate that the placement has been executed
    });
  }
}
