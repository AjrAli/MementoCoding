import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.css']
})
export class ConfirmModalComponent {
  @Input() title!: string;
  @Input() errorMessage!: ErrorResponse;
  constructor(public modal: NgbActiveModal) {}

}
