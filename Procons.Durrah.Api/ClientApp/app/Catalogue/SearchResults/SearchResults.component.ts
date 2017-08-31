import { Component, OnInit, Input, EventEmitter, Output, Sanitizer } from '@angular/core';
import { Worker } from '../../Models/Worker';
import { MenuItem } from 'primeng/primeng';

@Component({
  selector: 'search-result',
  templateUrl: './SearchResults.component.html',
  styleUrls: ['./SearchResults.component.css']
})
export class SearchResultsComponent implements OnInit {
  @Input() workers: Worker[];
  @Output() onSelectedWorker = new EventEmitter<Worker>();

  constructor(public sanitizer: Sanitizer) { }

  ngOnInit() {
    console.log('SearchResultsComponent Loaded! ', this.workers);
  }
  GoToProfile(selectedWorker: Worker) {
    console.log('[captured] GoToProfile!: ', selectedWorker);
    this.onSelectedWorker.emit(selectedWorker);
  }
  GetAvailableCSS(worker: Worker) {
    var isAvaible = worker.status == "1" ? true : false;
    return {
      "glyphicon": true,
      "glyphicon-ok": isAvaible,
      "glyphicon-remove": !isAvaible
    };
  }
  public openRequestedPopup(url) {
    console.log('opening url ',url);
    if (!url) {
      url = "https://www.youtube.com/watch?v=TzqxmgwcKUs";
    }
    var strWindowFeatures = "menubar=yes,location=yes,resizable=yes,scrollbars=yes,status=yes";
    return window.open(url, "Video", strWindowFeatures);
  }


}
