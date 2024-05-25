import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface Course {
  id: string;
  title: string;
  quota: number;
  specialties: number[];
  faculty: number;
  studyComponent: number;
}

@Injectable({
  providedIn: 'root'
})
export class CoursesService {
  private apiUrl = 'http://localhost:5000/api/Courses';

  constructor(private http: HttpClient) {}

  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(this.apiUrl);
  }

  getCourse(id: string): Observable<Course> {
    return this.http.get<Course>(`${this.apiUrl}/${id}`);
  }

  addCourse(course: string): Observable<void> {
    return this.http.post<void>(this.apiUrl, course);
  }

  addCourses(courses: string[]): Observable<Course[]> {
    return this.http.post<Course[]>(`${this.apiUrl}/add-courses`, courses);
  }

  getCoursePlacement(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/placement`);
  }

  unenrollEveryone(): Observable<void> {
    return this.http.get<void>(`${this.apiUrl}/unenroll-everyone`);
  }
}
