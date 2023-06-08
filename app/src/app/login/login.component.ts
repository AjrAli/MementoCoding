import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/authentification/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorResponse } from '../dto/error/error-response';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  errorMessage!: ErrorResponse;
  constructor(private authService: AuthenticationService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
  }
  onSubmit() {
    this.authService.authenticate(this.username, this.password).subscribe({
      next: (r) => {
        if (r.token) {
          this.authService.setToken(r.token);
          // Récupérer l'URL demandée avant la connexion
          const returnUrl = this.route.snapshot.queryParams['returnUrl'];

          // Rediriger vers l'URL demandée ou vers '/home' par défaut
          this.router.navigateByUrl(returnUrl || '/home');
        } else {
          alert('Invalid credentials');
        }
      },
      error: (e) => {
        if (e.status === 400 || e.status === 404) {
          this.errorMessage = e.error as ErrorResponse;
          console.error(this.errorMessage);
        } else {
          console.error('Error:', e);
          alert('Error: ' + e.message);
        }
      },
      complete: () => console.info('complete')
    });
  }
}