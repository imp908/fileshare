import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { App6Component } from './app6.component';

describe('App6Component', () => {
  let component: App6Component;
  let fixture: ComponentFixture<App6Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ App6Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(App6Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
