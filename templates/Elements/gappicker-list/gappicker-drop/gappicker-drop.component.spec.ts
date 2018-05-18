import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GappickerDropComponent } from './gappicker-drop.component';

describe('GappickerDropComponent', () => {
  let component: GappickerDropComponent;
  let fixture: ComponentFixture<GappickerDropComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GappickerDropComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GappickerDropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
