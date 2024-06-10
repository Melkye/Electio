import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode'; // Import jwt_decode from jwt-decode library

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5207/api/Auth'; // Replace with your actual API URL

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  get isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  get token(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    const token = this.token;
    if (token) {
      const decodedToken: any = jwtDecode(token); // Decode the token using jwt_decode
      console.log('Decoded token:', decodedToken)
      return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    }
    return null;
  }

  getStudentId(): string {
    if (!this.token) return '';
    const decodedToken: any = jwtDecode(this.token);
    return decodedToken.studentId; // Assuming the user ID is stored in the 'jit' claim
  }
}
