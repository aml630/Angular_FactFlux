import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TwitterUsersComponent } from './twitter-users.component';

describe('TwitterUsersComponent', () => {
  let component: TwitterUsersComponent;
  let fixture: ComponentFixture<TwitterUsersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TwitterUsersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TwitterUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
