import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { StudentsComponent } from './students.component';
import { TableComponent } from '../components/table/table.component';
import { ConfirmModalComponent } from '../modals/confirm-modal/confirm-modal.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
describe('StudentsComponent', () => {
  let component: StudentsComponent;
  let fixture: ComponentFixture<StudentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, NgbPaginationModule], // Importez HttpClientTestingModule
      declarations: [StudentsComponent, TableComponent, ConfirmModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
