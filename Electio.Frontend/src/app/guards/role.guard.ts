import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = this.authService.token;

    console.log("Token in role guard:", token);

    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        console.log("Decoded token: ", decodedToken)
        const expectedRole = next.data['expectedRole'];

        console.log("some expected role:", next.data['expectedRole'])

        if (decodedToken && decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === expectedRole) {
          return true;
        }
      } catch (e) {
        console.error('Error decoding token', e);
      }
    }

    this.router.navigate(['/login']);
    return false;
  }
}
