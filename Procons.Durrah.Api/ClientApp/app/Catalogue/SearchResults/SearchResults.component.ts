import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import {Worker} from '../../Models/Worker';
import { MenuItem } from 'primeng/primeng';

@Component({
  selector: 'search-result',
  templateUrl: './SearchResults.component.html',
  styleUrls: ['./SearchResults.component.css']
})
export class SearchResultsComponent implements OnInit {
  @Input() workers: Worker[];
  @Output() onSelectedWorker = new EventEmitter<Worker>();

  constructor() { }

  ngOnInit() {
    console.log('SearchResultsComponent Loaded! ', this.workers);
  }
  GoToProfile(selectedWorker: Worker) {
    console.log('[captured] GoToProfile!: ', selectedWorker);
    this.onSelectedWorker.emit(selectedWorker);
  }
}
