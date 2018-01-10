import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkerExperienceComponent } from './worker-experience.component';

describe('WorkerExperienceComponent', () => {
  let component: WorkerExperienceComponent;
  let fixture: ComponentFixture<WorkerExperienceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkerExperienceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkerExperienceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
