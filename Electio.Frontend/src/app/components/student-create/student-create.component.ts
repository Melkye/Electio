import { Component } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'student-create',
  templateUrl: './student-create.component.html'
})
export class StudentCreateComponent {
  student = { username: '', email: '', password: '' };

  constructor(private adminService: AdminService) {}

  createStudent() {
    this.adminService.createStudent(this.student).subscribe(response => {
      console.log('Student created successfully', response);
    }, error => {
      console.error('Error creating student', error);
    });
  }
}
