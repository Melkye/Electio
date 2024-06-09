// course-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Course } from '../../models/course.model';
import { CourseEnrollment } from '../../models/course-enrollment.model';
import { CoursesService } from '../../services/courses.service';
import { concatMap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { CourseFormComponent } from '../course-form/course-form.component';
import { LoadingDialogComponent } from '../loading-dialog/loading-dialog.component'; 

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
[x: string]: any;
  courses: Course[] = [];
  courseEnrollments: CourseEnrollment[] = [];
  isPlacementExecuted = false;
  usedAlgorithm = "";
  placementEfficiency = 0;

  constructor(private coursesService: CoursesService,  private dialog: MatDialog) {}
  
  ngOnInit(): void {
    this.loadCourses();
    this.loadCoursePlacements();
    
    this.coursesService.setIsPlacementExecuted().subscribe(isExecuted => {
      console.log('Placement status received: ', isExecuted);
      this.isPlacementExecuted = isExecuted;
      if (this.isPlacementExecuted) {
        this.loadPlacementEfficiency();
      }
    });
    

  }

  loadCourses(): void {
    this.coursesService.getCourses().subscribe(courses => {
      // TODO: update sorting when studyConponent will be string
      this.courses = courses.sort((a, b) => a.studyComponent - b.studyComponent);
    });
  }

  loadCoursePlacements(): void {
    this.coursesService.getAllCoursesPlacement().subscribe(enrollments => {
      this.courseEnrollments = enrollments;
    });
  }

  getEnrollmentsForCourse(course: Course): CourseEnrollment | undefined {
    return this.courseEnrollments.find(enrollment => enrollment.title === course.title);
  }

  unenrollEveryone(): void {
    this.coursesService.unenrollEveryone().subscribe(() => {
      console.log('Everyone unenrolled');
      this.ngOnInit()
    });
  }

  runGBPAlgorithm(): void {
    const dialogRef = this.dialog.open(LoadingDialogComponent, {
      disableClose: true // Prevents closing the dialog by clicking outside
    });

    this.coursesService.unenrollEveryone().pipe(
      concatMap(() => {
        console.log('Everyone unenrolled');
        this.usedAlgorithm = 'GBP';
        return this.coursesService.executeGradeBiasedPlacement();
      })
    ).subscribe({
      next: () => {
        console.log('GBP algorithm executed');
        this.ngOnInit();
      },
      complete: () => {
        dialogRef.close();
      },
      error: (error) => {
        console.error('Error executing GBP algorithm:', error);
        dialogRef.close();
      }
    });
  }
  

  runATBPAlgorithm(): void {
    const dialogRef = this.dialog.open(LoadingDialogComponent, {
      disableClose: true // Prevents closing the dialog by clicking outside
    });

    this.coursesService.unenrollEveryone().pipe(
      concatMap(() => {
        console.log('Everyone unenrolled');
        this.usedAlgorithm = 'ATBP';
        return this.coursesService.executeAccessTimeBiasedPlacement();
      })
    ).subscribe({
      next: () => {
        console.log('ATBP algorithm executed');
        this.ngOnInit();
      },
      complete: () => {
        dialogRef.close();
      },
      error: (error) => {
        console.error('Error executing ATBP algorithm:', error);
        dialogRef.close();
      }
    });
  }

  loadPlacementEfficiency(): void {
    this.coursesService.getPlacementEficiency().subscribe(efficiency => {
      this.placementEfficiency = Number(efficiency);
    });
  }

  addCourse(): void {
    const dialogRef = this.dialog.open(CourseFormComponent, {
      data: { course: null }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.courses.push(result);
        this.loadCourses();
      }
    });
  }

  editCourse(course: Course): void {
    const dialogRef = this.dialog.open(CourseFormComponent, {
      data: { course }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const index = this.courses.findIndex(c => c.id === result.id);
        if (index !== -1) {
          this.courses[index] = result;
        }
      }
    });
  }

  deleteCourse(courseId: string): void {
    this.coursesService.deleteCourse(courseId).subscribe(() => {
      const course = this.courses.find(course => course.id == courseId);
      console.log('Course deleted successfully with id:', courseId, "title", course!.title);
      this.courses = this.courses.filter(course => course.id !== courseId);
    }, error => {
      console.error('Error deleting course:', error);
    });
  }

  setRandomPriorities(): void {

    const dialogRef = this.dialog.open(LoadingDialogComponent, {
      disableClose: true // Prevents closing the dialog by clicking outside
    });

    this.coursesService.setRandomPriorities().subscribe({
      next: () => {
        console.log('Random priorities set');
        //this.ngOnInit();
      },
      complete: () => {
        dialogRef.close();
      },
      error: (error) => {
        console.error('Error setting random priorities:', error);
        dialogRef.close();
      }
    });
  }
}
