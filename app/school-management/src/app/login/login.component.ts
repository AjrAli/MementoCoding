import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentification/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private authService: AuthenticationService, private router: Router) { }

  onSubmit() {
    this.authService.authenticate(this.username, this.password).subscribe(
      (response: any) => {
        if (response.token) {
          this.authService.setToken(response.token);
          this.router.navigate(['/schools']);
        } else {
          alert('Invalid credentials');
        }
      },
      (error: any) => {
        alert('Error: ' + error.message);
      }
    );
  }
}