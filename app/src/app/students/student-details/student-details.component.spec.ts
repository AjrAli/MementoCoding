import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentDetailsComponent } from './student-details.component';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';
import { ToastService } from 'src/app/services/message-popup/toast.service';
describe('StudentDetailsComponent', () => {
  let component: StudentDetailsComponent;
  let fixture: ComponentFixture<StudentDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StudentDetailsComponent],
      imports: [RouterTestingModule.withRoutes([]), HttpClientTestingModule, ToastrModule.forRoot(), // Only import ToastrModule once
      ],
      providers: [ToastService],
    })
      .compileComponents();

    fixture = TestBed.createComponent(StudentDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
