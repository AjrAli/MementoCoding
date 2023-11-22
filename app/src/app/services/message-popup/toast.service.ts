import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor(private toastr: ToastrService) {}

  showSuccess(message: string): void {
    this.showNotification('success', message, 'Success', 3000);
  }

  showError(errorResponse: ErrorResponse): void {
    const errorMessageArray: string[] = [errorResponse?.message, ...(errorResponse?.validationErrors ?? [])];

    errorMessageArray.forEach((message) => {
      if (message) {
        this.showNotification('error', message, 'Error', 5000);
      }
    });
  }
  showSimpleError(message: string): void {
    this.showNotification('error', message, 'Error', 5000);
  }
  private showNotification(type: 'success' | 'error', message: string, title: string, duration: number): void {
    const options = {
      positionClass: 'toast-bottom-center',
      timeOut: duration
    };

    if (type === 'success') {
      this.toastr.success(message, title, options);
    } else {
      this.toastr.error(message, title, options);
    }
  }
}
