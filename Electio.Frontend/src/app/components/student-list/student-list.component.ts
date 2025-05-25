import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
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
  displayedColumns: string[] = [
    'name',
    'averageGrade',
    'faculty',
    'specialty',
    //'group',
    'studyYear',
  ];
  filter: string = '';

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  constructor(private studentService: StudentsService) {}

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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
  
  getFacultyName(number: number): string {
    if (number === 1) {
      return 'ФІОТ';
    } else if (number === 2) {
      return 'ІПСА';
    } else {
      return 'Unknown Faculty';
    }
  }
}
