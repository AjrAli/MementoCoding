import { ChangeDetectorRef, Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DtoModalComponent } from 'src/app/modals/dto-modal/dto-modal.component';
import { ToastService } from '../message-popup/toast.service';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';
import { ConfirmModalComponent } from 'src/app/modals/confirm-modal/confirm-modal.component';
import { BaseDto } from 'src/app/dto/utilities/base-dto';
import { BaseResponse } from 'src/app/dto/response/base-response';

@Injectable({
  providedIn: 'root',
})
export class ModalService {

  constructor(private modalWindowService: NgbModal,
    private toastService: ToastService) { }

  async openDtoModal(dto: any, title: string, actionFn: (receivedDto: any) => Promise<any>) {
    const modalRef = this.modalWindowService.open(DtoModalComponent);
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.dto = dto;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe({
      next: async (receivedDto: any) => {
        const result = await actionFn(receivedDto);
        if (result?.success) {
          this.toastService.showSuccess(result.message);
          modalRef.componentInstance.doClearForm();
          modalRef.close();
        } else {
          let responseError = result as ErrorResponse;
          this.toastService.showError(responseError);
        }
      },
      error: (error: ErrorResponse) => {
        this.toastService.showError(error);
      },
      complete: () => console.info('complete')
    });
  }

  async openConfirmModal(parameter: any, checkToDelete: boolean, title: string, methodCallIfYesButton: (parameter: any) => Promise<any>) {
    const modalRef = this.modalWindowService.open(ConfirmModalComponent);
    modalRef.componentInstance.title = title;
    if (checkToDelete) {
      const errorMessage: ErrorResponse = {
        message: `(Impossible to delete ${title}, some elements are linked to it!)`,
        success: false,
        validationErrors: []
      };
      modalRef.componentInstance.errorMessage = errorMessage;
    }
    modalRef.result.then(async (r) => {
      if (r === 'yes' && !checkToDelete) {
        const yesButtonResult = await methodCallIfYesButton(parameter) as BaseResponse;
        if (yesButtonResult?.success) {
          this.toastService.showSuccess(yesButtonResult.message);
        } else {
          let responseError = yesButtonResult as ErrorResponse;
          this.toastService.showError(responseError);
        }
      } else {
        console.log('Action annulée');
      }
    }).catch((error) => {
      this.toastService.showError(error as ErrorResponse);
    });
  }
  

}
