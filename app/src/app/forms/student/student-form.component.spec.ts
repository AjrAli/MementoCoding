import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { StudentFormComponent } from './student-form.component';
import { ToastrModule } from 'ngx-toastr';
import { ToastService } from 'src/app/services/message-popup/toast.service';

describe('StudentFormComponent', () => {
  let component: StudentFormComponent;
  let fixture: ComponentFixture<StudentFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule, HttpClientTestingModule, ToastrModule.forRoot(),
      ],
      providers: [ToastService], // Importez les modules nécessaires ici
      declarations: [StudentFormComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(StudentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
