import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSellerItemComponent } from './user-seller-item.component';

describe('UserSellerItemComponent', () => {
  let component: UserSellerItemComponent;
  let fixture: ComponentFixture<UserSellerItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserSellerItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserSellerItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
