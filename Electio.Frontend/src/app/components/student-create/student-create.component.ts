import { Component } from '@angular/core';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'student-create',
  templateUrl: './student-create.component.html',
  styleUrls: ['./student-create.component.css']
})
export class StudentCreateComponent {
  student = {
    name: '',
    username: '',
    email: '',
    password: '',
    averageGrade: 100,
    specialty: 121,
    faculty: 1,
    studyYear: 1
  };

  constructor(private adminService: AdminService) {}

  createStudent() {
    this.adminService.createStudent(this.student).subscribe(response => {
      console.log('Student created successfully', response);
    }, error => {
      console.error('Error creating student', error);
    });
  }
}
