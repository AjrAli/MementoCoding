import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/authentification/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorResponse } from '../dto/response/error/error-response';
import { ToastService } from '../services/message-popup/toast.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  errorResponse!: ErrorResponse;
  constructor(private authService: AuthenticationService,
              private router: Router, 
              private route: ActivatedRoute,
              private toastService: ToastService) { }

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
          this.toastService.showSuccess(r.message);
          // Rediriger vers l'URL demandée ou vers '/home' par défaut
          this.router.navigateByUrl(returnUrl || '/home');
        } else {
          alert('Invalid credentials');
        }
      },
      error: (e) => {
        this.errorResponse = e.error as ErrorResponse;
        if (e.status === 400 || e.status === 404) {         
          console.error(this.errorResponse);
        } else {
          console.error('Error:', e);
          alert('Error: ' + e.message);
        }
        this.toastService.showError(this.errorResponse);
      },
      complete: () => console.info('complete')
    });
  }
}