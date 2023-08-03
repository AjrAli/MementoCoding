import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidationErrorComponent } from './form-validation-error.component';

describe('FormValidationErrorComponent', () => {
  let component: FormValidationErrorComponent;
  let fixture: ComponentFixture<FormValidationErrorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FormValidationErrorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormValidationErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
