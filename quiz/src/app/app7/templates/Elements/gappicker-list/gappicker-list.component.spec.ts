import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GappickerListComponent } from './gappicker-list.component';

describe('GappickerListComponent', () => {
  let component: GappickerListComponent;
  let fixture: ComponentFixture<GappickerListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GappickerListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GappickerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
