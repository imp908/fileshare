import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GappickerNgComponent } from './gappicker-ng.component';

describe('GappickerNgComponent', () => {
  let component: GappickerNgComponent;
  let fixture: ComponentFixture<GappickerNgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GappickerNgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GappickerNgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
