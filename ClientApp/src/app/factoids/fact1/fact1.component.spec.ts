import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Fact1Component } from './fact1.component';

describe('Fact1Component', () => {
  let component: Fact1Component;
  let fixture: ComponentFixture<Fact1Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Fact1Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Fact1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
