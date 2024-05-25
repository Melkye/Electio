import { Component, OnInit } from '@angular/core';
import { CoursesService } from '../../services/courses.service';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
})
export class CourseListComponent implements OnInit {
  courses: any[] = [];

  constructor(private coursesService: CoursesService) {}

  ngOnInit(): void {
    this.coursesService.getCourses().subscribe(data => {
      this.courses = data;
    });
  }
}
