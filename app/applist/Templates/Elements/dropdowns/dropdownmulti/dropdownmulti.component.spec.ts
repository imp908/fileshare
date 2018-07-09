import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropdownmultiComponent } from './dropdownmulti.component';

describe('DropdownmultiComponent', () => {
  let component: DropdownmultiComponent;
  let fixture: ComponentFixture<DropdownmultiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropdownmultiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropdownmultiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
