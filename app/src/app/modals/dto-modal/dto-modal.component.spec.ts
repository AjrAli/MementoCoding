import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DtoModalComponent } from './dto-modal.component';

describe('DtoModalComponent', () => {
  let component: DtoModalComponent;
  let fixture: ComponentFixture<DtoModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NgbModule],
      declarations: [DtoModalComponent],
      providers: [NgbActiveModal], // Ajoutez le fournisseur NgbActiveModal ici
    }).compileComponents();

    fixture = TestBed.createComponent(DtoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});