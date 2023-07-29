import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchoolDetailsComponent } from './school-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TableComponent } from 'src/app/components/table/table.component';
import { ToastrModule } from 'ngx-toastr';
import { ToastService } from 'src/app/services/message-popup/toast.service';

describe('SchoolDetailsComponent', () => {
  let component: SchoolDetailsComponent;
  let fixture: ComponentFixture<SchoolDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SchoolDetailsComponent, TableComponent],
      imports: [RouterTestingModule.withRoutes([]), HttpClientTestingModule, ToastrModule.forRoot(),
      ],
      providers: [ToastService],
    })
      .compileComponents();

    fixture = TestBed.createComponent(SchoolDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
