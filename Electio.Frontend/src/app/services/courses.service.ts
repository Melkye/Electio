import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, defer, map, shareReplay } from 'rxjs';
import { Student } from '../models/student.model';
import { Course } from '../models/course.model';


// interface Course {
//   id: string;
//   title: string;
//   quota: number;
//   specialties: number[];
//   faculty: number;
//   studyComponent: number;
// }

@Injectable({
  providedIn: 'root'
})
export class CoursesService {
  private apiUrl = 'http://localhost:5207/api/Courses';

  // // TODO: make endpoint on back anr tetrieve the result via function
  // public isPlacementExecuted(): boolean { 
  //   return this.getAllCoursesPlacement().subscribe(enrollments => {
  //     return enrollments.length > 0;
  //   });
  // }

  constructor(private http: HttpClient) {}

  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(this.apiUrl);
  }

  // async getCourses(): Promise<Course[]> {
  //   return this.http.get<Course[]>(this.apiUrl).toPromise() as Promise<Course[]>;
  // }

  // TODO: implement on back
  // getCourse(id: string): Observable<Course> {
  //   return this.http.get<Course>(`${this.apiUrl}/${id}`);
  // }

  // TODO: make typesafe
  getCoursePlacement(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/placement/${id}`);
  }

  addCourse(course: string): Observable<void> {
    return this.http.post<void>(this.apiUrl, course);
  }

  addCourses(courses: string[]): Observable<Course[]> {
    return this.http.post<Course[]>(`${this.apiUrl}/add-courses`, courses);
  }

  getAllCoursesPlacement(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/placement`);
  }

  unenrollEveryone(): Observable<void> {
    return this.http.get<void>(`${this.apiUrl}/unenroll-everyone`, {});
  }

  executeAccessTimeBiasedPlacement(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl.replace('Courses', 'Students')}/execute-time-biased-placement`, {});
  }

  executeGradeBiasedPlacement(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl.replace('Courses', 'Students')}/execute-grade-biased-placement`, {});
  }

  getCourseId(courseTitle: string): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/get-id-by-title/${courseTitle}`);
  }
  
  setIsPlacementExecuted(): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/placement-status`);
  }

  getPlacementEficiency(): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/placement-efficiency`);
  }
}
