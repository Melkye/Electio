import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StudentsService } from '../../services/students.service';

@Component({
  selector: 'app-student-detail',
  templateUrl: './student-detail.component.html',
})
export class StudentDetailComponent implements OnInit {
  student: any;

  constructor(private studentsService: StudentsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id') ?? '';
    this.studentsService.getStudent(id).subscribe(data => {
      this.student = data;
    });
  }
}
