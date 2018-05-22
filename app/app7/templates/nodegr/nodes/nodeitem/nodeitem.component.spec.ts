import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NodeitemComponent } from './nodeitem.component';

describe('NodeitemComponent', () => {
  let component: NodeitemComponent;
  let fixture: ComponentFixture<NodeitemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NodeitemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NodeitemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
