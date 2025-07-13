import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBuyerComponent } from './user-buyer.component';

describe('UserBuyerComponent', () => {
  let component: UserBuyerComponent;
  let fixture: ComponentFixture<UserBuyerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBuyerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBuyerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
