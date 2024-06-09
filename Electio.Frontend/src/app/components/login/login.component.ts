import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  credentials = { username: '', password: '' };

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.authService.login(this.credentials).subscribe(response => {
      localStorage.setItem('token', response.token);
      const role = this.authService.getRole();

      // TODO: make use of this separation
      if (role === 'admin') {
        this.router.navigate(['/courses']);
      } else if (role === 'student') {
        this.router.navigate(['/courses']);
      }
    });
  }
}
