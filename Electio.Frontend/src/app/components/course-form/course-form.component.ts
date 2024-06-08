import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Course } from '../../models/course.model'; 
import { CoursesService } from '../../services/courses.service';

@Component({
  selector: 'app-course-form',
  templateUrl: './course-form.component.html',
  styleUrls: ['./course-form.component.css']
})
export class CourseFormComponent implements OnInit {
  courseForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CourseFormComponent>,
    private coursesService: CoursesService,
    @Inject(MAT_DIALOG_DATA) public data: { course: Course },
  ) {}

  ngOnInit(): void {
    this.courseForm = this.fb.group({
      //id: [this.data.course?.id , Validators.required],
      title: [this.data.course?.title || '', Validators.required],
      quota: [this.data.course?.quota || 0, [Validators.required, Validators.min(1)]],
      faculty: [this.data.course?.faculty || 0, Validators.required],
      specialties: [this.data.course?.specialties || [], Validators.required],
      studyComponent: [this.data.course?.studyComponent || 0, Validators.required]
    });
  }

  onSave(): void {
    if (this.courseForm.valid) {
      const courseData = this.courseForm.value;
      // Assuming your service method is named 'saveCourse'
      this.coursesService.addCourse(courseData).subscribe(
        response => {
          console.log('Course saved successfully:', response);
          this.dialogRef.close(response); // Close the dialog after saving
        },
        error => {
          console.error('Error saving course:', error);
          // Handle error accordingly (e.g., display error message)
        }
      );
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
