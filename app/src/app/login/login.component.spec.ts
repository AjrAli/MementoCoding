import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthenticationService } from '../services/authentification/authentication.service';
import { LoginComponent } from './login.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormValidationErrorComponent } from '../shared/validation/form-validation-error/form-validation-error.component';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginComponent, FormValidationErrorComponent ],
      imports: [HttpClientModule, RouterTestingModule, FormsModule, ReactiveFormsModule, HttpClientTestingModule, ToastrModule.forRoot(), ToastrModule],
      providers: [AuthenticationService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
