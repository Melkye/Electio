import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'http://localhost:5207/api/admin';

  constructor(private http: HttpClient) {}

  createStudent(student: { username: string, email: string, password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/create-student`, student);
  }
}