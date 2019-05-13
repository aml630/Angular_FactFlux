import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PostEventComponent } from './post-event.component';

describe('PostEventComponent', () => {
  let component: PostEventComponent;
  let fixture: ComponentFixture<PostEventComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostEventComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
