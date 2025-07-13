import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBuyerItemComponent } from './user-buyer-item.component';

describe('UserBuyerItemComponent', () => {
  let component: UserBuyerItemComponent;
  let fixture: ComponentFixture<UserBuyerItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBuyerItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBuyerItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
