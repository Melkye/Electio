import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  credentials = { username: '', password: '' };

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (localStorage.getItem('token'))
      this.router.navigate(['/courses']);
  }

  login() {
    this.authService.login(this.credentials).subscribe(response => {
      localStorage.setItem('token', response.token);
      const role = this.authService.getRole();

      // TODO: make use of this separation
      if (role === 'admin' || role === 'Admin') {
        this.router.navigate(['/students']);
      } else if (role === 'student') {
        this.router.navigate(['/courses']);
      }
    });
  }
}
