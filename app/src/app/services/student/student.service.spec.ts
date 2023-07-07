import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { StudentService } from './student.service';

describe('StudentService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule], // Importer HttpClientModule ici
      providers: [StudentService],
    });
  });

  it('should be created', () => {
    const service: StudentService = TestBed.inject(StudentService);
    expect(service).toBeTruthy();
  });
});