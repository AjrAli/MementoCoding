import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TableComponent } from './table.component';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
describe('TableComponent', () => {
  let component: TableComponent;
  let fixture: ComponentFixture<TableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NgbPaginationModule, HttpClientTestingModule, NgbPaginationModule, RouterTestingModule.withRoutes([])],
      declarations: [ TableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
