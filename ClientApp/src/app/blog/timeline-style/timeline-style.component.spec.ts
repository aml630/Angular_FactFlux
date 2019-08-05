import { async, ComponentFixture, TestBed } from 'src/app/blog/timeline-style/node_modules/@angular/core/testing';

import { TimelineStyleComponent } from './timeline-style.component';

describe('TimelineStyleComponent', () => {
  let component: TimelineStyleComponent;
  let fixture: ComponentFixture<TimelineStyleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimelineStyleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimelineStyleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
