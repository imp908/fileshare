import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NodesGroups } from './nodes.component';

describe('NodesGroups', () => {
  let component: NodesGroups;
  let fixture: ComponentFixture<NodesGroups>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NodesGroups ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NodesGroups);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
