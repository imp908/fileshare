import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PasspageComponent } from './passpage.component';

describe('PasspageComponent', () => {
  let component: PasspageComponent;
  let fixture: ComponentFixture<PasspageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PasspageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PasspageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
