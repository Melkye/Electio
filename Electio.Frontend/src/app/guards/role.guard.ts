import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { jwtDecode } from 'jwt-decode'; // Import jwt_decode from jwt-decode library

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const token = this.authService.token;

    if (token) {
      const decodedToken: any = jwtDecode(token); // Decode the token using jwt_decode

      const expectedRole = next.data['expectedRole'];

      if (decodedToken && decodedToken.role === expectedRole) {
        return true;
      }
    }

    this.router.navigate(['/login']);
    return false;
  }
}
