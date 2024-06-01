// course-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Course } from '../../models/course.model';
import { CourseEnrollment } from '../../models/course-enrollment.model';
import { CoursesService } from '../../services/courses.service';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  courses: Course[] = [];
  courseEnrollments: CourseEnrollment[] = [];

  constructor(private coursesService: CoursesService) {}
  ngOnInit(): void {
    this.loadCourses();
    this.loadCourseEnrollments();
  }

  loadCourses(): void {
    this.coursesService.getCourses().subscribe(courses => {
      this.courses = courses;
    });
  }

  loadCourseEnrollments(): void {
    this.coursesService.getCoursePlacement().subscribe(enrollments => {
      this.courseEnrollments = enrollments;
    });
  }

  getEnrollmentsForCourse(course: Course): CourseEnrollment | undefined {
    return this.courseEnrollments.find(enrollment => enrollment.title === course.title);
  }
}
