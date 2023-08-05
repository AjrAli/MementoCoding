import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthenticationResponse, AuthenticationService } from '../services/authentification/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorResponse } from '../dto/response/error/error-response';
import { ToastService } from '../services/message-popup/toast.service';
import { AccountDto } from '../dto/account/account-dto';
import { firstValueFrom } from 'rxjs';
import { ModalService } from '../services/modal/modal.service';
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
    private toastService: ToastService,
    private modalService: ModalService) { }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
  }

  async openAddModal(): Promise<void> {
    await this.modalService.openDtoModal(new AccountDto(), 'Account Modal', this.createAccount.bind(this));
  }

  async createAccount(account: AccountDto): Promise<AuthenticationResponse> {
    try {
      const response: AuthenticationResponse = await firstValueFrom(this.authService.createSimpleUser(account));
      if (response?.token) {
        this.authService.setToken(response?.token);
        const returnUrl = this.route.snapshot.queryParams['returnUrl'];
        this.router.navigateByUrl(returnUrl || '/home');
      } else {
        throw new ErrorResponse('Invalid credentials', false);
      }
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to create account. Please try again later.');
      throw e;
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
          this.toastService.showSimpleError('Invalid credentials');
        }
      },
      error: (e) => {
        this.errorResponse = e.error as ErrorResponse;
        this.toastService.showError(this.errorResponse);
        this.toastService.showSimpleError('Invalid credentials');
      },
      complete: () => console.info('complete')
    });
  }
}