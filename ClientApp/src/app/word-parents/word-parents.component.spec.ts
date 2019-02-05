import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WordParentsComponent } from './word-parents.component';

describe('WordParentsComponent', () => {
  let component: WordParentsComponent;
  let fixture: ComponentFixture<WordParentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WordParentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WordParentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
