import { TestBed } from '@angular/core/testing';

import { SchoolService } from './school.service';
import { HttpClientModule } from '@angular/common/http';

describe('SchoolService', () => {
  let service: SchoolService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule], // Importer HttpClientModule ici
      providers: [SchoolService],
    });
  });

  it('should be created', () => {
    const service: SchoolService = TestBed.inject(SchoolService);
    expect(service).toBeTruthy();
  });
});
