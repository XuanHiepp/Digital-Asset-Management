import { TestBed } from '@angular/core/testing';

import { MusicItemService } from './music-item.service';

describe('MusicItemService', () => {
  let service: MusicItemService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MusicItemService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
