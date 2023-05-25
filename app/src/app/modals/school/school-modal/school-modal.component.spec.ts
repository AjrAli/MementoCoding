import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchoolModalComponent } from './school-modal.component';

describe('SchoolModalComponent', () => {
  let component: SchoolModalComponent;
  let fixture: ComponentFixture<SchoolModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SchoolModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SchoolModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
