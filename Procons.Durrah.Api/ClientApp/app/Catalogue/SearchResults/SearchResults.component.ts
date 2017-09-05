import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Worker } from '../../Models/Worker';
import { ProconsModalSerivce } from '../../Services/ProconsModalService';
import { MenuItem } from 'primeng/primeng';

@Component({
  selector: 'search-result',
  templateUrl: './SearchResults.component.html',
  styleUrls: ['./SearchResults.component.css']
})
export class SearchResultsComponent implements OnInit {
  @Input() workers: Worker[];
  @Output() onSelectedWorker = new EventEmitter<Worker>();
  public showVideoModal: boolean = false;
  public videoUrl;

  constructor(public sanitizer: DomSanitizer, public myModal: ProconsModalSerivce) { }

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
    console.log('opening url ', url);
    this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    this.showVideoModal = true;
  }



}
