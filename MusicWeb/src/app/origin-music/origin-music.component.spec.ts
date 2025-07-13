import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OriginMusicComponent } from './origin-music.component';

describe('OriginMusicComponent', () => {
  let component: OriginMusicComponent;
  let fixture: ComponentFixture<OriginMusicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OriginMusicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OriginMusicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
