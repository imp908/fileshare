import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropdownmultngComponent } from './dropdownmulting.component';

describe('DropdownmultngComponent', () => {
  let component: DropdownmultngComponent;
  let fixture: ComponentFixture<DropdownmultngComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropdownmultngComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropdownmultngComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
