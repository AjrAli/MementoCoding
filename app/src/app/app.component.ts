import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './services/authentification/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'school-management';
  constructor(private router: Router, private authenticationService: AuthenticationService) {}

  isConnected() {
    return localStorage.getItem("authToken");
  }
  logout(): void {
    console.debug(localStorage);
    localStorage.removeItem('authToken');
    this.router.navigate(['/home']);
  }
}
