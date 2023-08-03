import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SchoolFormComponent } from './school-form.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormValidationErrorComponent } from 'src/app/shared/validation/form-validation-error/form-validation-error.component';

describe('SchoolFormComponent', () => {
  let component: SchoolFormComponent;
  let fixture: ComponentFixture<SchoolFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule, HttpClientTestingModule], // Importez les modules nécessaires ici
      declarations: [SchoolFormComponent, FormValidationErrorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SchoolFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});