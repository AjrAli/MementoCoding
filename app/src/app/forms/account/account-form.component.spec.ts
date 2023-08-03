import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountFormComponent } from './account-form.component';
import { FormValidationErrorComponent } from 'src/app/shared/validation/form-validation-error/form-validation-error.component';
import { ReactiveFormsModule } from '@angular/forms';


describe('AccountFormComponent', () => {
  let component: AccountFormComponent;
  let fixture: ComponentFixture<AccountFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [AccountFormComponent, FormValidationErrorComponent], // Add FormControlErrorDirective to declarations
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  // Other test cases go here...
});
