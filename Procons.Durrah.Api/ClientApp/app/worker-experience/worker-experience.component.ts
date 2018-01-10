import { Component, OnInit,Input } from '@angular/core';

@Component({
  selector: 'worker-experience',
  templateUrl: './worker-experience.component.html',
  styleUrls: ['./worker-experience.component.css']
})
export class WorkerExperienceComponent implements OnInit {
  @Input() worker;

  constructor() { }
  ngOnInit() {
    console.log('got worker experience of worker',this.worker);
  }

}
