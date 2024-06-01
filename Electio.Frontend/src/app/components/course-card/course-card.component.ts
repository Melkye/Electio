// course-card.component.ts
import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Course } from '../../models/course.model';
import { Student } from '../../models/student.model';

@Component({
  selector: 'app-course-card',
  templateUrl: './course-card.component.html',
  styleUrls: ['./course-card.component.css']
})
export class CourseCardComponent {
  @Input() course!: Course;
  students: Student[] = [];

  constructor(private http: HttpClient) {}

  loadStudents(): void {
    this.http.get<Student[]>(`http://localhost:5207/api/courses/${this.course.id}/placement`).subscribe(
      (students) => {
        this.students = students;
      },
      (error) => {
        console.error('Failed to load students', error);
      }
    );
  }
}
