import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Student } from '../models/student.model';

@Injectable({
  providedIn: 'root'
})
export class StudentsService {
  private apiUrl = 'http://localhost:5207/api/Students';

  constructor(private http: HttpClient) {}

  getStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.apiUrl);
  }

  getStudent(id: string): Observable<Student> {
    return this.http.get<Student>(`${this.apiUrl}/${id}`);
  }

  addStudent(student: Student): Observable<Student> {
    return this.http.post<Student>(this.apiUrl, student);
  }

  addStudents(students: Student[]): Observable<Student[]> {
    return this.http.post<Student[]>(`${this.apiUrl}/add-students`, students);
  }

  setRandomPriorities(): Observable<any[]> {
    return this.http.post<any[]>(`${this.apiUrl}/set-random-priorities`, {});
  }

  executePlacement(): Observable<Student[]> {
    return this.http.post<Student[]>(`${this.apiUrl}/execute-placement`, {});
  }
}
