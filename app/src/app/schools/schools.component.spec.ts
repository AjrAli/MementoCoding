import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SchoolsComponent } from './schools.component';
import { TableComponent } from '../components/table/table.component';
import { ConfirmModalComponent } from '../modals/confirm-modal/confirm-modal.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
describe('SchoolsComponent', () => {
  let component: SchoolsComponent;
  let fixture: ComponentFixture<SchoolsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, NgbPaginationModule, RouterTestingModule.withRoutes([])], // Importez HttpClientTestingModule
      declarations: [SchoolsComponent, TableComponent, ConfirmModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SchoolsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});