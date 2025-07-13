import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSellerComponent } from './user-seller.component';

describe('UserSellerComponent', () => {
  let component: UserSellerComponent;
  let fixture: ComponentFixture<UserSellerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserSellerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserSellerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
