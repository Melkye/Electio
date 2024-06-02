import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CoursesService } from '../../services/courses.service';
import { Student } from '../../models/student.model';
import { Course } from '../../models/course.model';
import { CourseEnrollment } from '../../models/course-enrollment.model';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
})
export class CourseDetailComponent implements OnInit {
  course: Course | undefined;
  students: Student[] = [];
  courseEnrollment: CourseEnrollment | undefined;

  constructor(private coursesService: CoursesService, private route: ActivatedRoute) {}


  
  // Due to API returns not course but placement -- array of students,
  // THe logic here deviates from the 'normal' flow
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id') ?? '';

    this.loadStudents(id)

    // this.coursesService.getPlacement(id).subscribe(data => {
    //   this.students = data;
    //   console.log(data);
    // });

  }

  loadStudents(courseId: string): void {
    this.coursesService.getCoursePlacement(courseId).subscribe(coursePlacementInfo => {
      this.students = coursePlacementInfo.students;
      console.log(coursePlacementInfo);
    });
  }
}
