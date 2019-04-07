import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MainWordsComponent } from './main-words.component';

describe('MainWordsComponent', () => {
  let component: MainWordsComponent;
  let fixture: ComponentFixture<MainWordsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MainWordsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainWordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
