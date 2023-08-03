import { Component, OnInit } from '@angular/core';
import { AuthenticationResponse, AuthenticationService } from '../services/authentification/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorResponse } from '../dto/response/error/error-response';
import { ToastService } from '../services/message-popup/toast.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DtoModalComponent } from '../modals/dto-modal/dto-modal.component';
import { AccountDto } from '../dto/account/account-dto';
import { firstValueFrom } from 'rxjs';
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
    private _modalService: NgbModal) { }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
  }

  async openAddModal(): Promise<void> {
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'Account Modal';
    modalRef.componentInstance.dto = new AccountDto();
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe(async (receivedAccount: AccountDto) => {
      const result = await this.createAccount(receivedAccount);
      if (result?.token) {
        this.authService.setToken(result?.token);
        // Récupérer l'URL demandée avant la connexion
        const returnUrl = this.route.snapshot.queryParams['returnUrl'];
        this.toastService.showSuccess(result?.message);
        modalRef.componentInstance.doClearForm();
        modalRef.close();
        // Rediriger vers l'URL demandée ou vers '/home' par défaut
        this.router.navigateByUrl(returnUrl || '/home');
      } else {
        this.toastService.showSimpleError('Invalid credentials');
      }
    });
  }

  async createAccount(account: AccountDto): Promise<AuthenticationResponse> {
    try {
      const response: AuthenticationResponse = await firstValueFrom(this.authService.createSimpleUser(account));
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