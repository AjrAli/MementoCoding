import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SchoolsComponent } from './schools.component';
import { TableComponent } from '../components/table/table.component';
import { ConfirmModalComponent } from '../modals/confirm-modal/confirm-modal.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { ToastService } from '../services/message-popup/toast.service';
import { ReactiveFormsModule } from '@angular/forms';

describe('SchoolsComponent', () => {
  let component: SchoolsComponent;
  let fixture: ComponentFixture<SchoolsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        NgbPaginationModule,
        ReactiveFormsModule,
        RouterTestingModule.withRoutes([]),
        ToastrModule.forRoot(), // Only import ToastrModule once
      ],
      providers: [ToastService],
      declarations: [SchoolsComponent, TableComponent, ConfirmModalComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SchoolsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});