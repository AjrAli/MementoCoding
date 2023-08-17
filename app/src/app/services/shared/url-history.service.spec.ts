import { TestBed } from '@angular/core/testing';

import { UrlHistoryService } from './url-history.service';

describe('UrlHistoryService', () => {
  let service: UrlHistoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UrlHistoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
