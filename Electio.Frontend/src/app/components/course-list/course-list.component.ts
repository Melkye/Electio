// course-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Course } from '../../models/course.model';
import { CourseEnrollment } from '../../models/course-enrollment.model';
import { CoursesService } from '../../services/courses.service';
import { concatMap } from 'rxjs';

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

  constructor(private coursesService: CoursesService) {}
  
  ngOnInit(): void {
    this.loadCourses();
    this.loadCoursePlacements();
    
    this.coursesService.setIsPlacementExecuted().subscribe(isExecuted => {
      console.log('Placement status received');
      this.isPlacementExecuted = isExecuted;
    });

    this.loadPlacementEfficiency();
  }

  loadCourses(): void {
    this.coursesService.getCourses().subscribe(courses => {
      this.courses = courses;
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
    this.coursesService.unenrollEveryone().pipe(
      concatMap(() => {
        console.log('Everyone unenrolled');
        this.usedAlgorithm = 'GBP';
        return this.coursesService.executeGradeBiasedPlacement();
      })
    ).subscribe(() => {
      console.log('GBP algorithm executed');
      this.ngOnInit()
    });
  }

  runATBPAlgorithm(): void {
    this.coursesService.unenrollEveryone().pipe(
      concatMap(() => {
        console.log('Everyone unenrolled');
        this.usedAlgorithm = 'ATBP';
        return this.coursesService.executeAccessTimeBiasedPlacement();
      })
    ).subscribe(() => {
      console.log('ATBP algorithm executed');
      
      this.ngOnInit()
    });
  }

  loadPlacementEfficiency(): void {
    this.coursesService.getPlacementEficiency().subscribe(efficiency => {
      this.placementEfficiency = Number(efficiency);
    });
  }
}
