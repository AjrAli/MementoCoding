import { TestBed } from '@angular/core/testing';

import { ModalService } from './modal.service';
import { ToastService } from '../message-popup/toast.service';
import { ToastrModule } from 'ngx-toastr';

describe('ModalService', () => {
  let service: ModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        ToastrModule.forRoot(), // Only import ToastrModule once
      ],
      providers: [ToastService]
    });
    service = TestBed.inject(ModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
