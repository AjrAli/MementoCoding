import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DtoModalComponent } from './dto-modal.component';

describe('DtoModalComponent', () => {
  let component: DtoModalComponent;
  let fixture: ComponentFixture<DtoModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DtoModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DtoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
