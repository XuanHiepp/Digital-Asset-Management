import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckKeyComponent } from './check-key.component';

describe('CheckKeyComponent', () => {
  let component: CheckKeyComponent;
  let fixture: ComponentFixture<CheckKeyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckKeyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckKeyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
