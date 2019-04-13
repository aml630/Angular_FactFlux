import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FactoidsComponent } from './factoids.component';

describe('FactoidsComponent', () => {
  let component: FactoidsComponent;
  let fixture: ComponentFixture<FactoidsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FactoidsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FactoidsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
