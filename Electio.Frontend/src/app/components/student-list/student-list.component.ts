import { Component, OnInit } from '@angular/core';
import { StudentsService } from '../../services/students.service';

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
})
export class StudentListComponent implements OnInit {
  students: any[] = [];

  constructor(private studentsService: StudentsService) { }

  ngOnInit(): void {
    this.studentsService.getStudents().subscribe(data => {
      this.students = data;
    });
  }
}
