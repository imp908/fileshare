import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestHtmlItemComponent } from './test-html-item.component';

describe('TestHtmlItemComponent', () => {
  let component: TestHtmlItemComponent;
  let fixture: ComponentFixture<TestHtmlItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestHtmlItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestHtmlItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
