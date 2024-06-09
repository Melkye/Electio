import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5207/api/auth'; // Replace with your actual API URL

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService,
    private router: Router
  ) {}

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
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken.role;
    }
    return null;
  }
}


// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Router } from '@angular/router';
// import { Observable } from 'rxjs';
// import { tap } from 'rxjs/operators';

// @Injectable({
//   providedIn: 'root'
// })
// export class AuthService {
//   private apiUrl = 'http://localhost:5207/api/auth';

//   constructor(private http: HttpClient, private router: Router) { }

//   login(credentials: any): Observable<any> {
//     return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
//       tap(response => {
//         localStorage.setItem('token', response.token);
//       })
//     );
//   }

//   logout(): void {
//     localStorage.removeItem('token');
//     this.router.navigate(['/login']);
//   }

//   get isAuthenticated(): boolean {
//     return !!localStorage.getItem('token');
//   }

//   get token(): string | null {
//     return localStorage.getItem('token');
//   }

//   getRole(): string | null {
//     const token = this.token;
//     if (token && !this.jwtHelper.isTokenExpired(token)) {
//       const decodedToken = this.jwtHelper.decodeToken(token);
//       return decodedToken.role;
//     }
//     return null;
//   }
// }
