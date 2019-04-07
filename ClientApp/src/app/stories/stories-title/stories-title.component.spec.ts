import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StoriesTitleComponent } from './stories-title.component';

describe('StoriesTitleComponent', () => {
  let component: StoriesTitleComponent;
  let fixture: ComponentFixture<StoriesTitleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StoriesTitleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StoriesTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
