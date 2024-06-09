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
  oldCourse: Course | undefined;
  allCourses: Course[] = [];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CourseFormComponent>,
    private coursesService: CoursesService,
    @Inject(MAT_DIALOG_DATA) public data: { course: Course },
  ) {}

  ngOnInit(): void {
      this.oldCourse = this.data.course;
      console.log('Creating or uodating course:');
      console.log('OldCourse:', this.oldCourse);

    this.courseForm = this.fb.group({
      //id: [this.allCourses.find(c => c.id == this.oldCourse?.id)?.id || ''],
      title: [this.data.course?.title || '', Validators.required],
      quota: [this.data.course?.quota || 0, [Validators.required, Validators.min(1)]],
      faculty: [this.data.course?.faculty || 0, Validators.required],
      specialties: [this.data.course?.specialties || [], Validators.required],
      studyComponent: [this.data.course?.studyComponent || 0, Validators.required]
    });

    this.coursesService.getCourses().subscribe(courses => {
      this.allCourses = courses;
      console.log('AllCourses:', courses);
    });
  }

  onSave(): void {
    if (this.courseForm.valid) {
      if(this.oldCourse?.id) {
        const id = this.oldCourse!.id;
        const courseData = this.courseForm.value;
        this.coursesService.updateCourse(id, courseData).subscribe(
          response => {
            console.log('Course updated successfully:', response);
            this.dialogRef.close(response);
          },
          error => {
            console.error('Error updating course:', error);
          }
        );
      }
      else {
        const courseData = this.courseForm.value;
        this.coursesService.addCourse(courseData).subscribe(
          response => {
            console.log('Course saved successfully:', response);
            this.dialogRef.close(response);
          },
          error => {
            console.error('Error saving course:', error);
          }
        );
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
