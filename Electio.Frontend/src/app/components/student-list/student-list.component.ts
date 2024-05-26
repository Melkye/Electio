// student-list.component.ts
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Student } from '../../models/student.model';
import { StudentsService } from '../../services/students.service';

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements OnInit {
  dataSource = new MatTableDataSource<Student>();
  displayedColumns: string[] = ['name', 'grade']; // Add more column names if needed
  filter: string = '';

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;

  constructor(private studentService: StudentsService) {}

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.fetchStudents();
  }

  fetchStudents(): void {
    this.studentService.getStudents()
      .subscribe(students => {
        this.dataSource.data = students;
      });
  }

  applyFilter(): void {
    this.dataSource.filter = this.filter.trim().toLowerCase();
  }
}
