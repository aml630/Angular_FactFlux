import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WordTimelineComponent } from './word-timeline.component';

describe('WordTimelineComponent', () => {
  let component: WordTimelineComponent;
  let fixture: ComponentFixture<WordTimelineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WordTimelineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WordTimelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
